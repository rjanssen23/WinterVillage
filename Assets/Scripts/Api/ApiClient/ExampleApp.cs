using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleApp : MonoBehaviour
{
    [Header("Test data")]
    public User user;
    public ProfielKeuze profielKeuze;

    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public ProfielkeuzeApiClient profielkeuzeApiClient;

    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                // TODO: Handle succes scenario;
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login()
    {
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                // TODO: Todo handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion

    #region ProfielKeuze

    [ContextMenu("ProfielKeuze/Read all")]
    public async void ReadProfielKeuzes()
    {
        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.ReadProfielKeuzes();

        switch (webRequestResponse)
        {
            case WebRequestData<List<ProfielKeuze>> dataResponse:
                List<ProfielKeuze> profielKeuzes = dataResponse.Data;
                Debug.Log("List of profielKeuzes: ");
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read profielKeuzes error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("ProfielKeuze/Create")]
    public async void CreateProfielKeuze()
    {
        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.CreateProfielKeuze(profielKeuze);

        switch (webRequestResponse)
        {
            case WebRequestData<ProfielKeuze> dataResponse:
                //profielKeuze.id = dataResponse.Data.id;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create profielKeuze error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    //[ContextMenu("ProfielKeuze/Delete")]
    //public async void DeleteProfielKeuze()
    //{
    //    //IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.DeleteProfielKeuze(profielKeuze.id);

    //    switch (webRequestResponse)
    //    {
    //        case WebRequestData<string> dataResponse:
    //            string responseData = dataResponse.Data;
    //            // TODO: Handle succes scenario.
    //            break;
    //        case WebRequestError errorResponse:
    //            string errorMessage = errorResponse.ErrorMessage;
    //            Debug.Log("Delete profielKeuze error: " + errorMessage);
    //            // TODO: Handle error scenario. Show the errormessage to the user.
    //            break;
    //        default:
    //            throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
    //    }
    //}

    #endregion ProfielKeuze

    #region Agenda
    public class AgendaApiClient : MonoBehaviour
    {
        public WebClient webClient;

        public async Awaitable<IWebRequestReponse> ReadAgendas()
        {
            string route = "/api/agenda"; // Correct route
            IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
            return ParseAgendaListResponse(webRequestResponse);
        }

        public async Awaitable<IWebRequestReponse> CreateAgenda(Agenda agenda)
        {
            string route = "/api/agenda"; // Correct route
            string data = JsonUtility.ToJson(agenda);
            IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
            return ParseAgendaResponse(webRequestResponse);
        }

        public async Awaitable<IWebRequestReponse> DeleteAgenda(string agendaId)
        {
            string route = "/api/agenda/" + agendaId; // Correct route
            return await webClient.SendDeleteRequest(route);
        }

        public async Awaitable<IWebRequestReponse> UpdateAgenda(string agendaId, Agenda agenda)
        {
            string route = "/api/agenda/" + agendaId; // Correct route
            string data = JsonUtility.ToJson(agenda);
            IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
            return ParseAgendaResponse(webRequestResponse);
        }

        private IWebRequestReponse ParseAgendaResponse(IWebRequestReponse webRequestResponse)
        {
            switch (webRequestResponse)
            {
                case WebRequestData<string> data:
                    Debug.Log("Response data raw: " + data.Data);
                    Agenda agenda = JsonUtility.FromJson<Agenda>(data.Data);
                    WebRequestData<Agenda> parsedWebRequestData = new WebRequestData<Agenda>(agenda);
                    return parsedWebRequestData;
                default:
                    return webRequestResponse;
            }
        }

        private IWebRequestReponse ParseAgendaListResponse(IWebRequestReponse webRequestResponse)
        {
            switch (webRequestResponse)
            {
                case WebRequestData<string> data:
                    Debug.Log("Response data raw: " + data.Data);
                    List<Agenda> agendas = JsonHelper.ParseJsonArray<Agenda>(data.Data);
                    WebRequestData<List<Agenda>> parsedWebRequestData = new WebRequestData<List<Agenda>>(agendas);
                    return parsedWebRequestData;
                default:
                    return webRequestResponse;
            }
        }

        public async void GetAllAgendaInfo()
        {
            Debug.LogError("Agendas are being loaded");
            IWebRequestReponse response = await ReadAgendas();
            if (response is WebRequestData<List<Agenda>> agendasData)
            {
                List<Agenda> agendas = agendasData.Data;
                foreach (var agenda in agendas)
                {
                    Debug.Log($"Date1: {agenda.date1}, Location1: {agenda.location1}, Date2: {agenda.date2}, Location2: {agenda.location2}, Date3: {agenda.date3}, Location3: {agenda.location3}");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve agendas.");
            }
        }
    }
    #endregion
}


#region Object2D

//[ContextMenu("Object2D/Read all")]
//public async void ReadObject2Ds()
//{
//    IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(object2D.environmentId);

//    switch (webRequestResponse)
//    {
//        case WebRequestData<List<Object2D>> dataResponse:
//            List<Object2D> object2Ds = dataResponse.Data;
//            Debug.Log("List of object2Ds: " + object2Ds);
//            object2Ds.ForEach(object2D => Debug.Log(object2D.id));
//            // TODO: Succes scenario. Show the enviroments in the UI
//            break;
//        case WebRequestError errorResponse:
//            string errorMessage = errorResponse.ErrorMessage;
//            Debug.Log("Read object2Ds error: " + errorMessage);
//            // TODO: Error scenario. Show the errormessage to the user.
//            break;
//        default:
//            throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//    }
//}

//[ContextMenu("Object2D/Create")]
//public async void CreateObject2D()
//{
//    IWebRequestReponse webRequestResponse = await object2DApiClient.CreateObject2D(object2D);

//    switch (webRequestResponse)
//    {
//        case WebRequestData<Object2D> dataResponse:
//            object2D.id = dataResponse.Data.id;
//            // TODO: Handle succes scenario.
//            break;
//        case WebRequestError errorResponse:
//            string errorMessage = errorResponse.ErrorMessage;
//            Debug.Log("Create Object2D error: " + errorMessage);
//            // TODO: Handle error scenario. Show the errormessage to the user.
//            break;
//        default:
//            throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//    }
//}

//[ContextMenu("Object2D/Update")]
//public async void UpdateObject2D()
//{
//    IWebRequestReponse webRequestResponse = await object2DApiClient.UpdateObject2D(object2D);

//    switch (webRequestResponse)
//    {
//        case WebRequestData<string> dataResponse:
//            string responseData = dataResponse.Data;
//            // TODO: Handle succes scenario.
//            break;
//        case WebRequestError errorResponse:
//            string errorMessage = errorResponse.ErrorMessage;
//            Debug.Log("Update object2D error: " + errorMessage);
//            // TODO: Handle error scenario. Show the errormessage to the user.
//            break;
//        default:
//            throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//    }
//}

#endregion
