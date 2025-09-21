using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataSystem
{
    public static string SAVE_FILE_PATH = Application.persistentDataPath + "/save.adun";

    public static void SaveProgData(MetaprogressionData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SAVE_FILE_PATH, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static MetaprogressionData LoadProgData()
    {
        if (File.Exists(SAVE_FILE_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SAVE_FILE_PATH, FileMode.Open);
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
