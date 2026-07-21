using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SaveData
{
    public SerializableDictionary<string, int> selectedAvatar;
    public SerializableDictionary<string, int> selectedAccessory;
    public SerializableDictionary<string, string> selectedColor;

    public readonly void AddColor(string name, Color color)
    {
        if (!selectedColor.ContainsKey(name))
            selectedColor.Add(name, ColorUtility.ToHtmlStringRGBA(color));
        else
            selectedColor[name] = ColorUtility.ToHtmlStringRGBA(color);
    }

    public readonly void AddColor(string name, string color)
    {
        if (!selectedColor.ContainsKey(name))
            selectedColor.Add(name, color);
        else
            selectedColor[name] = color;
    }

    public readonly void AddAvatar(string name, int index)
    {
        if (!selectedAvatar.ContainsKey(name))
            selectedAvatar.Add(name, index);
        else
            selectedAvatar[name] = index;
    }

    public readonly void AddAccessory(string name, int index)
    {
        if (!selectedAccessory.ContainsKey(name))
            selectedAccessory.Add(name, index);
        else
            selectedAccessory[name] = index;
    }

    public readonly int? GetAvatar(string name)
    {
        if (selectedAvatar.ContainsKey(name))
            return selectedAvatar[name];
        else
            return null;
    }

    public readonly int? GetAccessory(string name)
    {
        if (selectedAccessory.ContainsKey(name))
            return selectedAccessory[name];
        else
            return null;
    }

    public readonly string GetColor(string name)
    {
        if (selectedColor.ContainsKey(name))
        {
            return selectedColor[name];
        }
        else
            return null;
    }

    public void ClearUser(string name)
    {
        if (selectedAvatar.ContainsKey(name))
            selectedAvatar.Remove(name);
        if (selectedAccessory.ContainsKey(name))
            selectedAccessory.Remove(name);
        if (selectedColor.ContainsKey(name))
            selectedColor.Remove(name);
    }

    public static SaveData LoadSaveData(string saveDataPath)
    {
        try
        {
            string json = File.ReadAllText(saveDataPath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (Exception exception)
        {
            Debug.LogError("Error occurred new savedata, " + exception);
            var newSaveData = new SaveData()
            {
                selectedAvatar = new SerializableDictionary<string, int>(),
                selectedAccessory = new SerializableDictionary<string, int>(),
                selectedColor = new SerializableDictionary<string, string>()
            };
            SaveSaveData(saveDataPath, newSaveData);
            return newSaveData;
        }
    }

    public static void SaveSaveData(string saveDataPath, SaveData saveData)
    {
        try
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(saveDataPath, json);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }
}
