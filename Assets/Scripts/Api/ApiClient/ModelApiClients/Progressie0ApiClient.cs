using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Progressie0ApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Task<IWebRequestReponse> ReadProgressies(string profielkeuzeid)
    {
        string route = $"/api/progressie0/profielkeuze/{profielkeuzeid}"; // Use profielkeuzeid in the route
        Debug.Log($"Sending GET request to: {route}");
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        Debug.Log($"Received response: {webRequestResponse}");
        return ParseProgressieListResponse(webRequestResponse);
    }

    public async Task<IWebRequestReponse> CreateProgressie(string profielkeuzeid, Progressie0 progressie)
    {
        string route = $"/api/progressie0"; // Use the base route for creating a new progressie
        string data = JsonUtility.ToJson(progressie);
        Debug.Log($"Sending POST request to: {route} with data: {data}");
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        Debug.Log($"Received response: {webRequestResponse}");
        return ParseProgressieResponse(webRequestResponse);
    }

    public async Task<IWebRequestReponse> DeleteProgressie(string profielkeuzeid, string progressieId)
    {
        string route = $"/api/progressie0/{progressieId}"; // Use the base route for deleting a progressie
        Debug.Log($"Sending DELETE request to: {route}");
        return await webClient.SendDeleteRequest(route);
    }

    public async Task<IWebRequestReponse> UpdateProgressie(string profielkeuzeid, string progressieId, Progressie0 progressie)
    {
        string route = $"/api/progressie0/{progressieId}"; // Use the base route for updating a progressie
        string data = JsonUtility.ToJson(progressie);
        Debug.Log($"Sending PUT request to: {route} with data: {data}");
        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data);
        Debug.Log($"Received response: {webRequestResponse}");
        return ParseProgressieResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseProgressieResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Progressie0 progressie = JsonUtility.FromJson<Progressie0>(data.Data);
                WebRequestData<Progressie0> parsedWebRequestData = new WebRequestData<Progressie0>(progressie);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseProgressieListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Progressie0> progressies = JsonHelper.ParseJsonArray<Progressie0>(data.Data);
                WebRequestData<List<Progressie0>> parsedWebRequestData = new WebRequestData<List<Progressie0>>(progressies);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    public async void GetAllProgressieInfo(string profielkeuzeid)
    {
        Debug.LogError("Progressies are being loaded");
        IWebRequestReponse response = await ReadProgressies(profielkeuzeid);
        if (response is WebRequestData<List<Progressie0>> progressiesData)
        {
            List<Progressie0> progressies = progressiesData.Data;
            foreach (var progressie in progressies)
            {
                Debug.Log($"ID: {progressie.id}, Vakje1: {progressie.vakje1}, Vakje2: {progressie.vakje2}, Vakje3: {progressie.vakje3}, Vakje4: {progressie.vakje4}, Vakje5: {progressie.vakje5}, Vakje6: {progressie.vakje6}");
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve progressies.");
        }
    }
}


