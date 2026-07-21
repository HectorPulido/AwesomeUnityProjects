using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GameData
{
    public Texture2D background;
    public Texture2D foreground;
    public Texture2D[] defaultAvatar;
    public Texture2D[] defaultAvatarAccessory;
    public SerializableDictionary<string, Texture2D> avatars;
    public SerializableDictionary<string, Color> colors;

    private static Texture2D LoadTexture(string path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new(1, 1);
            loadTexture.LoadImage(bytes);
            return loadTexture;
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
            return null;
        }
    }

    public static GameData? LoadGameData(GameDataConfig gameDataConfig)
    {
        var gameData = new GameData();
        try
        {
            gameData.background = LoadTexture(gameDataConfig.backgroundPath);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
        try
        {
            gameData.foreground = LoadTexture(gameDataConfig.foregroundPath);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }

        if (gameDataConfig.defaultAvatarPath != null)
        {
            gameData.defaultAvatar = new Texture2D[gameDataConfig.defaultAvatarPath.Length];
            for (int i = 0; i < gameDataConfig.defaultAvatarPath.Length; i++)
            {
                try
                {
                    gameData.defaultAvatar[i] = LoadTexture(gameDataConfig.defaultAvatarPath[i]);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        if (gameDataConfig.defaultAvatarAccessory != null)
        {
            gameData.defaultAvatarAccessory = new Texture2D[gameDataConfig.defaultAvatarAccessory.Length];
            for (int i = 0; i < gameDataConfig.defaultAvatarAccessory.Length; i++)
            {
                try
                {
                    gameData.defaultAvatarAccessory[i] = LoadTexture(gameDataConfig.defaultAvatarAccessory[i]);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        if (gameDataConfig.avatarPaths != null)
        {
            gameData.avatars = new SerializableDictionary<string, Texture2D>();
            foreach (var avatarPath in gameDataConfig.avatarPaths)
            {
                try
                {
                    gameData.avatars.Add(avatarPath.Key, LoadTexture(avatarPath.Value));
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        if (gameDataConfig.posibleColors != null)
        {
            gameData.colors = new SerializableDictionary<string, Color>();
            foreach (var color in gameDataConfig.posibleColors)
            {
                try
                {
                    gameData.colors.Add(color.Key, ColorUtility.TryParseHtmlString(color.Value, out Color colorValue) ? colorValue : Color.white);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }
        return gameData;
    }
}
