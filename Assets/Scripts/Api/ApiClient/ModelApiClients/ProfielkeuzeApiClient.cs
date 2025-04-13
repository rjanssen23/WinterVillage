using System;
using System.Collections.Generic;
using UnityEngine;

public class ProfielkeuzeApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadProfielKeuzes()
    {
        string route = "/ProfielKeuze";
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseProfielKeuzeListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateProfielKeuze(ProfielKeuze profielKeuze)
    {
        string route = "/ProfielKeuze";
        string data = JsonUtility.ToJson(profielKeuze);
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseProfielKeuzeResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteProfielKeuze(string profielKeuzeId)
    {
        string route = "/ProfielKeuze/" + profielKeuzeId;
        return await webClient.SendDeleteRequest(route);
    }

    private IWebRequestReponse ParseProfielKeuzeResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                try
                {
                    ProfielKeuze profielKeuze = JsonUtility.FromJson<ProfielKeuze>(data.Data);
                    return new WebRequestData<ProfielKeuze>(profielKeuze);
                }
                catch (Exception e)
                {
                    Debug.LogError("Fout bij parsen van ProfielKeuze: " + e.Message);
                    return webRequestResponse;
                }
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseProfielKeuzeListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                try
                {
                    List<ProfielKeuze> profielKeuzes = JsonHelper.ParseJsonArray<ProfielKeuze>(data.Data);
                    return new WebRequestData<List<ProfielKeuze>>(profielKeuzes);
                }
                catch (Exception e)
                {
                    Debug.LogError("Fout bij parsen van ProfielKeuzes lijst: " + e.Message);
                    return webRequestResponse;
                }
            default:
                return webRequestResponse;
        }
    }

    public async void GetAllProfielInfo()
    {
        Debug.Log("Keuzes worden geladen...");
        IWebRequestReponse response = await ReadProfielKeuzes();
        if (response is WebRequestData<List<ProfielKeuze>> profielKeuzesData)
        {
            List<ProfielKeuze> profielKeuzes = profielKeuzesData.Data;
            foreach (var profielKeuze in profielKeuzes)
            {
                // Log alleen name en avatar, aangezien arts en geboortedatum niet langer nodig zijn.
                Debug.Log($"Name: {profielKeuze.name}, Avatar: {profielKeuze.avatar}");
            }
        }
        else if (response is WebRequestError error)
        {
            Debug.LogError("Fout bij ophalen profielkeuzes: " + error.ErrorMessage);
        }
        else
        {
            Debug.LogError("Onbekend antwoordtype bij ophalen profielkeuzes.");
        }
    }
}





