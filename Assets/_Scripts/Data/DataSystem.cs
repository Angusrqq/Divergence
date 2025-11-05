using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

// TODO: Implement this class because we cant save any data right now
/// <summary>
/// <para>
/// <c>DataSystem</c> is a static class that handles saving and loading of metaprogression data.
/// </para>
/// </summary>
public static class DataSystem
{
    public static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/save.adun";

    /// <summary>
    /// Saves the given metaprogression data to the file located at DataSystem.SAVE_FILE_PATH.
    /// </summary>
    /// <param name="data">The metaprogression data to be saved.</param>
    public static void SaveProgData(MetaprogressionData data)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(SAVE_FILE_PATH, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Loads metaprogression data from the file located at DataSystem.SAVE_FILE_PATH.
    /// If the file does not exist, returns null.
    /// </summary>
    /// <returns>The loaded metaprogression data or null if the file does not exist.</returns>
    public static MetaprogressionData LoadProgData()
    {
        if (File.Exists(SAVE_FILE_PATH))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(SAVE_FILE_PATH, FileMode.Open);

            MetaprogressionData data = formatter.Deserialize(stream) as MetaprogressionData;
            stream.Close();

            return data;
        }
        else
        {
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
