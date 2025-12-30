using UnityEngine;
using System.IO;
using System;
using MessagePack;

public static class DataSystem
{
    public static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/save.adun";

    public static void SaveProgressionData(MetaprogressionData data)
    {
        data.UpdateGuids();
        byte[] bytes = MessagePackSerializer.Serialize(data);

        File.WriteAllBytes(SAVE_FILE_PATH, bytes);
    }

    public static MetaprogressionData LoadProgressionData()
    {
        if (File.Exists(SAVE_FILE_PATH))
        {
            byte[] bytes = File.ReadAllBytes(SAVE_FILE_PATH);
            var data = MessagePackSerializer.Deserialize<MetaprogressionData>(bytes);

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }

    public static void SaveSettingsData(SettingsData data)
    {
        PlayerPrefs.SetString("Config", JsonUtility.ToJson(data));
    }

    public static SettingsData LoadSettingsData()
    {
        if (PlayerPrefs.HasKey("Config"))
        {
            return JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("Config"));
        }
        
        return null;
    }
}

[Serializable]
public class SettingsData
{
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
    public int ScreenWidth;
    public int ScreenHeight;
    public int RefreshRate;
    public string FullScreen;

    public SettingsData(float masterVolume = 0f, float musicVolume = 0f, float sfxVolume = 0f,
                        int screenWidth = default, int screenHeight = default, double refreshRate = default, string fullScreen = "Windowed")
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SfxVolume = sfxVolume;
        FullScreen = fullScreen;
        ScreenWidth = screenWidth == default ? Screen.currentResolution.width : screenWidth;
        ScreenHeight = screenHeight == default ? Screen.currentResolution.height : screenHeight;
        RefreshRate = (int)(refreshRate == default ? Screen.currentResolution.refreshRateRatio.value : refreshRate);
    }
}
