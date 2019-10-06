using System;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    private const float CLOSE_ENOUGH = 0.001f;

    private static Util instance;
    private static Util Instance => instance ?? (instance = new Util());

    private Dictionary<string, SongData> songs = new Dictionary<string, SongData>();

    private TYPE LoadData<TYPE>(string path, string id)
    {
        string filepath = path + "/" + id;
        if (PlayerPrefs.HasKey(filepath))
        {
            Debug.Log("Loading " + filepath + " from PlayerPrefs");
            TYPE data = JsonUtility.FromJson<TYPE>(PlayerPrefs.GetString(filepath));
            return data;
        }
        else
        {
            TextAsset json = Resources.Load<TextAsset>(filepath);
            if (json != null && !String.IsNullOrEmpty(json.text))
            {
                Debug.Log("Loading " + filepath + " from Resources");
                TYPE data = JsonUtility.FromJson<TYPE>(json.text);
                Resources.UnloadAsset(json);
                return data;
            }
        }

        Debug.LogWarning("Unable to find content for " + filepath);
        return default;
    }

    public static SongData GetSongData(string id)
    {
        if (!Instance.songs.ContainsKey(id))
            Instance.songs[id] = Instance.LoadData<SongData>("Music", id);
        return Instance.songs[id];
    }

    public static bool CloseEnough(float a, float b) { return a + CLOSE_ENOUGH > b && a - CLOSE_ENOUGH < b; }
}