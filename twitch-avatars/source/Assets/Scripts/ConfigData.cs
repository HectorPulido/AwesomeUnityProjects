using System.Collections;
using UnityEngine;

public class ConfigData : MonoBehaviour
{
    public string configDataPath = "config.json";
    public string saveDataPath = "save.json";
    public static GameDataConfig gameDataConfig;
    public static GameData gameData;
    public static SaveData saveData;

    public GameDataConfig gameDataConfigDebug;
    public GameData gameDataDebug;
    public SaveData saveDataDebug;

    public SpriteRenderer background;
    public SpriteRenderer foreground;

    public Camera mainCamera;

    void Awake()
    {
        gameDataConfig = GameDataConfig.LoadGameDataConfig(configDataPath).Value;
        gameData = GameData.LoadGameData(gameDataConfig).Value;
        saveData = SaveData.LoadSaveData(saveDataPath);

        gameDataConfigDebug = gameDataConfig;
        gameDataDebug = gameData;
        saveDataDebug = saveData;

        if (background != null && gameData.background != null)
        {
            background.sprite = Texture2DToSprite(gameData.background, 100, new Vector2(0.5f, 0));
        }

        if (foreground != null && gameData.foreground != null)
        {
            foreground.sprite = Texture2DToSprite(gameData.foreground, 100, new Vector2(0.5f, 0));
        }

        StartCoroutine(PeriodicSave());
    }

    public static Sprite Texture2DToSprite(Texture2D texture, float pixelsPerUnit = 350f, Vector2 pivot = default)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
    }

    public IEnumerator PeriodicSave()
    {
        while (true)
        {
            SaveData.SaveSaveData(saveDataPath, saveData);
            Debug.Log("Saved");
            yield return new WaitForSeconds(15);
        }
    }

}
