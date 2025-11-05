using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;

public class TilemapToPngExporterPrefab : EditorWindow
{
    private Tilemap targetTilemap;
    private string outputFolder = "Assets/Exports";
    private string baseFileName = "TilemapExport";
    private int pixelsPerUnit = 32;
    private int maxChunkSize = 8192;
    private bool createPrefab = true;

    [MenuItem("Tools/Tilemap/Export Tilemap → Split PNGs + Prefab")]
    public static void ShowWindow()
    {
        GetWindow<TilemapToPngExporterPrefab>("Tilemap Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilemap to PNG (with Prefab Export)", EditorStyles.boldLabel);
        targetTilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", targetTilemap, typeof(Tilemap), true);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        baseFileName = EditorGUILayout.TextField("Base File Name", baseFileName);
        pixelsPerUnit = EditorGUILayout.IntField("Pixels Per Unit", pixelsPerUnit);
        maxChunkSize = EditorGUILayout.IntField("Max Chunk Size", maxChunkSize);
        createPrefab = EditorGUILayout.Toggle("Create Prefab", createPrefab);

        GUILayout.Space(10);
        if (GUILayout.Button("Export"))
        {
            if (targetTilemap == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Tilemap to export.", "OK");
                return;
            }

            ExportTilemap();
        }
    }

    private void ExportTilemap()
    {
        var grid = targetTilemap.layoutGrid;
        var cellSize = grid.cellSize;
        BoundsInt bounds = targetTilemap.cellBounds;
        int totalWidth = bounds.size.x * pixelsPerUnit;
        int totalHeight = bounds.size.y * pixelsPerUnit;

        int chunksX = Mathf.CeilToInt((float)totalWidth / maxChunkSize);
        int chunksY = Mathf.CeilToInt((float)totalHeight / maxChunkSize);

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        // Hide other renderers during capture
        var allRenderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        foreach (var r in allRenderers)
            r.enabled = false;
        foreach (var r in targetTilemap.GetComponentsInChildren<Renderer>())
            r.enabled = true;

        GameObject parent = new GameObject(baseFileName + "_Baked");
        Vector3 origin = targetTilemap.CellToWorld(bounds.min);

        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                int chunkWidth = Mathf.Min(maxChunkSize, totalWidth - x * maxChunkSize);
                int chunkHeight = Mathf.Min(maxChunkSize, totalHeight - y * maxChunkSize);

                var rt = new RenderTexture(chunkWidth, chunkHeight, 24);
                var camGO = new GameObject("ExportCam");
                var cam = camGO.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = chunkHeight / (pixelsPerUnit * 2f);
                cam.clearFlags = CameraClearFlags.Color;
                cam.backgroundColor = Color.clear;
                cam.targetTexture = rt;

                // Camera position for this chunk
                Vector3 chunkCenter = new Vector3(
                    bounds.xMin + (x + 0.5f) * (maxChunkSize / (float)pixelsPerUnit),
                    bounds.yMin + (y + 0.5f) * (maxChunkSize / (float)pixelsPerUnit),
                    -10f
                );
                cam.transform.position = chunkCenter;

                cam.Render();

                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(chunkWidth, chunkHeight, TextureFormat.RGBA32, false);
                tex.ReadPixels(new Rect(0, 0, chunkWidth, chunkHeight), 0, 0);
                tex.Apply();

                string filePath = Path.Combine(outputFolder, $"{baseFileName}_x{x}_y{y}.png");
                File.WriteAllBytes(filePath, tex.EncodeToPNG());

                RenderTexture.active = null;
                rt.Release();
                DestroyImmediate(rt);
                DestroyImmediate(camGO);
                DestroyImmediate(tex);

                // Create sprite + GameObject
                AssetDatabase.Refresh();
                Texture2D importedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
                Sprite sprite = Sprite.Create(importedTex, new Rect(0, 0, importedTex.width, importedTex.height),
                                              new Vector2(0, 0), pixelsPerUnit);

                GameObject tilePart = new GameObject($"{baseFileName}_x{x}_y{y}");
                var sr = tilePart.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingOrder = 0;

                float worldX = origin.x + (x * (maxChunkSize / (float)pixelsPerUnit));
                float worldY = origin.y + (y * (maxChunkSize / (float)pixelsPerUnit));
                tilePart.transform.position = new Vector3(worldX, worldY, 0);
                tilePart.transform.SetParent(parent.transform, true);

                Debug.Log($"✅ Exported chunk {x},{y} to {filePath}");
            }
        }

        // Restore renderers
        foreach (var r in allRenderers)
            r.enabled = true;

        AssetDatabase.Refresh();

        if (createPrefab)
        {
            string prefabPath = Path.Combine(outputFolder, baseFileName + "_Baked.prefab");
            PrefabUtility.SaveAsPrefabAsset(parent, prefabPath);
            EditorUtility.DisplayDialog("Success", $"Tilemap exported and prefab saved to:\n{prefabPath}", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Success", $"Tilemap exported to PNGs and assembled in scene.", "OK");
        }

        Debug.Log($"🎨 Tilemap Export Completed: {chunksX * chunksY} chunks created.");
    }
}
