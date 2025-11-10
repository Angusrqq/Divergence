using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;

public class TilemapToChunkPrefabs : EditorWindow
{
    private Tilemap sourceTilemap;
    private int chunkSize = 64;
    private string outputFolder = "Assets/MapChunks";

    [MenuItem("Tools/Tilemap/Export to Chunk Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<TilemapToChunkPrefabs>("Tilemap Chunk Exporter");
    }

    void OnGUI()
    {
        GUILayout.Label("Tilemap to Chunk Prefabs", EditorStyles.boldLabel);
        sourceTilemap = (Tilemap)EditorGUILayout.ObjectField("Source Tilemap", sourceTilemap, typeof(Tilemap), true);
        chunkSize = EditorGUILayout.IntField("Chunk Size", chunkSize);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Export Tilemap to Prefabs"))
        {
            if (sourceTilemap == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Tilemap to export.", "OK");
                return;
            }
            ExportToPrefabs();
        }
    }

    private void ExportToPrefabs()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        Grid grid = sourceTilemap.layoutGrid;
        BoundsInt bounds = sourceTilemap.cellBounds;

        int minX = bounds.xMin;
        int minY = bounds.yMin;
        int maxX = bounds.xMax;
        int maxY = bounds.yMax;

        for (int y = minY; y < maxY; y += chunkSize)
        {
            for (int x = minX; x < maxX; x += chunkSize)
            {
                GameObject chunkObj = new GameObject($"Chunk_{x / chunkSize}_{y / chunkSize}");
                chunkObj.transform.position = grid.CellToWorld(new Vector3Int(x, y, 0));

                Tilemap tm = chunkObj.AddComponent<Tilemap>();
                TilemapRenderer tr = chunkObj.AddComponent<TilemapRenderer>();

                // Copy tiles
                for (int ty = y; ty < y + chunkSize; ty++)
                {
                    for (int tx = x; tx < x + chunkSize; tx++)
                    {
                        Vector3Int pos = new Vector3Int(tx, ty, 0);
                        TileBase tile = sourceTilemap.GetTile(pos);
                        if (tile != null)
                        {
                            tm.SetTile(new Vector3Int(tx - x, ty - y, 0), tile);
                        }
                    }
                }

                string localPath = $"{outputFolder}/Chunk_{x / chunkSize}_{y / chunkSize}.prefab";
                PrefabUtility.SaveAsPrefabAsset(chunkObj, localPath);
                DestroyImmediate(chunkObj);
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Tilemap successfully exported to prefabs!", "OK");
    }
}
