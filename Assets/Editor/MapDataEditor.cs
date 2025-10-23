using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDataEditor : EditorWindow
{
    private Tilemap sourceTilemap;
    private MapData targetMapData;

    [MenuItem("Tools/Tilemap Saver")]
    public static void ShowWindow()
    {
        GetWindow<MapDataEditor>("Tilemap Saver");
    }

    void OnGUI()
    {
        GUILayout.Label("Сохранение Tilemap в MapData", EditorStyles.boldLabel);

        sourceTilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap источник", sourceTilemap, typeof(Tilemap), true);
        targetMapData = (MapData)EditorGUILayout.ObjectField("Цель MapData", targetMapData, typeof(MapData), false);

        GUILayout.Space(10);

        if (sourceTilemap == null)
        {
            EditorGUILayout.HelpBox("Выберите Tilemap из сцены.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Создать новый MapData"))
        {
            CreateNewMapData();
        }

        UnityEngine.GUI.enabled = targetMapData != null;

        if (GUILayout.Button("Сохранить Tilemap в MapData"))
        {
            SaveTilemap();
        }

        if (GUILayout.Button("Применить MapData в Tilemap"))
        {
            ApplyTilemap();
        }

        UnityEngine.GUI.enabled = true;
    }

    private void CreateNewMapData()
    {
        string path = EditorUtility.SaveFilePanelInProject("Сохранить MapData", "NewMapData", "asset", "Выберите место для сохранения MapData");
        if (string.IsNullOrEmpty(path)) return;

        var newMapData = ScriptableObject.CreateInstance<MapData>();
        AssetDatabase.CreateAsset(newMapData, path);
        AssetDatabase.SaveAssets();
        targetMapData = newMapData;

        EditorUtility.DisplayDialog("MapData создан", "Файл MapData успешно создан!", "OK");
    }

    private void SaveTilemap()
    {
        if (sourceTilemap == null || targetMapData == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Выберите Tilemap и MapData!", "OK");
            return;
        }

        targetMapData.CaptureFromTilemap(sourceTilemap);
        targetMapData.mapName = sourceTilemap.name;
        EditorUtility.SetDirty(targetMapData);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Успех", $"Tilemap '{sourceTilemap.name}' сохранён в {targetMapData.name}.", "OK");
    }

    private void ApplyTilemap()
    {
        if (sourceTilemap == null || targetMapData == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Выберите Tilemap и MapData!", "OK");
            return;
        }

        Undo.RecordObject(sourceTilemap, "Apply MapData to Tilemap");
        targetMapData.ApplyToTilemap(sourceTilemap);
        EditorUtility.DisplayDialog("Применено", $"Карта '{targetMapData.name}' применена к Tilemap.", "OK");
    }
}
