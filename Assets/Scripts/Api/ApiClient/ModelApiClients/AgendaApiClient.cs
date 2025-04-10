using System.Collections.Generic;
using UnityEngine;

public class AgendaApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadAgendas(string profielkeuzeid)
    {
        string route = $"/api/agenda/profielkeuze/{profielkeuzeid}"; // Use profielkeuzeid in the route
        Debug.Log($"Sending GET request to: {route}");
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        Debug.Log($"Received response: {webRequestResponse}");
        return ParseAgendaListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateAgenda(string profielkeuzeid, Agenda agenda)
    {
        string route = $"/api/agenda"; // Use the base route for creating a new agenda
        string data = JsonUtility.ToJson(agenda);
        Debug.Log($"Sending POST request to: {route} with data: {data}");
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        Debug.Log($"Received response: {webRequestResponse}");
        return ParseAgendaResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteAgenda(string profielkeuzeid, string agendaId)
    {
        string route = $"/api/agenda/{agendaId}"; // Use the base route for deleting an agenda
        Debug.Log($"Sending DELETE request to: {route}");
        return await webClient.SendDeleteRequest(route);
    }

    public async Awaitable<IWebRequestReponse> UpdateAgenda(string profielkeuzeid, string agendaId, Agenda agenda)
    {
        string route = $"/api/agenda/{agendaId}"; // Use the base route for updating an agenda
        string data = JsonUtility.ToJson(agenda);
        Debug.Log($"Sending PUT request to: {route} with data: {data}");
        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data); // Use SendPutRequest instead of SendPostRequest
        Debug.Log($"Received response: {webRequestResponse}");
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

    public async void GetAllAgendaInfo(string profielkeuzeid)
    {
        Debug.LogError("Agendas are being loaded");
        IWebRequestReponse response = await ReadAgendas(profielkeuzeid);
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
