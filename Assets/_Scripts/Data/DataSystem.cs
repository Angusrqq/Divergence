using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// <para>
/// <c>DataSystem</c> is a static class that handles saving and loading of metaprogression data.
/// </para>
///TODO: Egor - Not implemented yet, not ready for use.
/// </summary>
public static class DataSystem
{
    public static string SAVE_FILE_PATH = Application.persistentDataPath + "/save.adun";

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
}
