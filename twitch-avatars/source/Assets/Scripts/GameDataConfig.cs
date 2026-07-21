using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GameDataConfig
{
    public string username;
    public string oauthToken;
    public string channelName;
    public string backgroundPath;
    public string foregroundPath;
    public string[] defaultAvatarPath;
    public string[] defaultAvatarAccessory;
    public SerializableDictionary<string, string> avatarPaths;
    public SerializableDictionary<string, string> posibleColors;

    public static GameDataConfig? LoadGameDataConfig(string configDataPath)
    {
        try
        {
            string json = File.ReadAllText(configDataPath);
            return JsonUtility.FromJson<GameDataConfig>(json);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
            return null;
        }
    }
}
