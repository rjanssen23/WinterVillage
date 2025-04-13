

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
