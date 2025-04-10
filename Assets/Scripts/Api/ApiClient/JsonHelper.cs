using System.Collections.Generic;
using System;
using UnityEngine;

public static class JsonHelper
{
    public static List<T> ParseJsonArray<T>(string jsonArray)
    {
        string extendedJson = "{\"list\":" + jsonArray + "}";
        JsonList<T> parsedList = JsonUtility.FromJson<JsonList<T>>(extendedJson);
        if (parsedList != null && parsedList.list != null)
        {
            Debug.Log($"Parsed list count: {parsedList.list.Count}");
        }
        else
        {
            Debug.LogError("Failed to parse JSON array.");
        }
        return parsedList.list;
    }

    public static string ExtractToken(string data)
    {
        Token token = JsonUtility.FromJson<Token>(data);
        return token.accessToken;
    }
}

[Serializable]
public class JsonList<T>
{
    public List<T> list;
}
