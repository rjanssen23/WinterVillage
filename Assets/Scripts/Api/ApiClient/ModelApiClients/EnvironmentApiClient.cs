using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadEnvironments()
    {
        string route = "/Environment";
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseEnvironmentListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateEnvironment(Environment environment)
    {
        string route = "/Environment";
        string data = JsonUtility.ToJson(environment);
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseEnvironmentResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteEnvironment(string environmentId)
    {
        string route = "/Environment/" + environmentId;
        return await webClient.SendDeleteRequest(route);
    }

    private IWebRequestReponse ParseEnvironmentResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                try
                {
                    Environment environment = JsonUtility.FromJson<Environment>(data.Data);
                    return new WebRequestData<Environment>(environment);
                }
                catch (Exception e)
                {
                    Debug.LogError("Fout bij parsen van Environment: " + e.Message);
                    return webRequestResponse;
                }
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseEnvironmentListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                try
                {
                    List<Environment> environments = JsonHelper.ParseJsonArray<Environment>(data.Data);
                    return new WebRequestData<List<Environment>>(environments);
                }
                catch (Exception e)
                {
                    Debug.LogError("Fout bij parsen van Environment lijst: " + e.Message);
                    return webRequestResponse;
                }
            default:
                return webRequestResponse;
        }
    }

    public async void GetAllEnvironmentInfo()
    {
        Debug.Log("Environments worden geladen...");
        IWebRequestReponse response = await ReadEnvironments();
        if (response is WebRequestData<List<Environment>> environmentData)
        {
            List<Environment> environments = environmentData.Data;
            foreach (var environment in environments)
            {
                Debug.Log($"Name: {environment.name}");
            }
        }
        else if (response is WebRequestError error)
        {
            Debug.LogError("Fout bij ophalen environments: " + error.ErrorMessage);
        }
        else
        {
            Debug.LogError("Onbekend antwoordtype bij ophalen environments.");
        }
    }
}
