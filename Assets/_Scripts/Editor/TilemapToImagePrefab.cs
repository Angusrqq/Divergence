using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class TilemapToImagePrefab : EditorWindow
{
    private Tilemap sourceTilemap;
    private string outputFolder = "Assets/BakedMaps";
    private int maxTextureSize = 4096; // Unity-safe default
    private int pixelsPerUnit = 32;

    [MenuItem("Tools/Tilemap/Bake to Image Prefab")]
    public static void ShowWindow()
    {
        GetWindow<TilemapToImagePrefab>("Tilemap to Image Prefab");
    }

    void OnGUI()
    {
        GUILayout.Label("Bake Tilemap to Image Prefab", EditorStyles.boldLabel);
        sourceTilemap = (Tilemap)EditorGUILayout.ObjectField("Source Tilemap", sourceTilemap, typeof(Tilemap), true);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        maxTextureSize = EditorGUILayout.IntField("Max Texture Size (px)", maxTextureSize);
        pixelsPerUnit = EditorGUILayout.IntField("Pixels Per Unit", pixelsPerUnit);

        GUILayout.Space(10);
        if (GUILayout.Button("Bake Tilemap"))
        {
            if (sourceTilemap == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Tilemap to bake.", "OK");
                return;
            }
            BakeTilemap();
        }
    }

    private void BakeTilemap()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        BoundsInt bounds = sourceTilemap.cellBounds;
        int mapWidth = bounds.size.x * pixelsPerUnit;
        int mapHeight = bounds.size.y * pixelsPerUnit;

        // how many tiles fit in one texture
        int chunkPixelSize = maxTextureSize;
        int chunksX = Mathf.CeilToInt((float)mapWidth / chunkPixelSize);
        int chunksY = Mathf.CeilToInt((float)mapHeight / chunkPixelSize);

        Grid grid = sourceTilemap.layoutGrid;
        Vector3Int cellSize = new Vector3Int(pixelsPerUnit, pixelsPerUnit, 0);

        List<Sprite> bakedSprites = new List<Sprite>();
        string prefabName = sourceTilemap.name + "_BakedMap";
        string prefabFolder = Path.Combine(outputFolder, prefabName);
        Directory.CreateDirectory(prefabFolder);

        Camera cam = new GameObject("TilemapBakeCam").AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = maxTextureSize / (2f * pixelsPerUnit);
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.clear;
        cam.enabled = false;
        cam.transform.position = new Vector3(0, 0, -10);

        int overlap = 2; // overlap pixels between chunks to avoid seams

        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                int startX = bounds.xMin + Mathf.RoundToInt((float)x * chunkPixelSize / pixelsPerUnit);
                int startY = bounds.yMin + Mathf.RoundToInt((float)y * chunkPixelSize / pixelsPerUnit);

                int chunkWidthPx = Mathf.Min(chunkPixelSize + overlap, mapWidth - x * chunkPixelSize + overlap);
                int chunkHeightPx = Mathf.Min(chunkPixelSize + overlap, mapHeight - y * chunkPixelSize + overlap);

                RenderTexture rt = new RenderTexture(chunkWidthPx, chunkHeightPx, 24);
                cam.targetTexture = rt;
                cam.orthographicSize = chunkHeightPx / (2f * pixelsPerUnit);

                // Calculate chunk center in world space (aligned to pixels)
                float worldCenterX = (startX * pixelsPerUnit + chunkWidthPx / 2f - overlap / 2f) / (float)pixelsPerUnit;
                float worldCenterY = (startY * pixelsPerUnit + chunkHeightPx / 2f - overlap / 2f) / (float)pixelsPerUnit;
                cam.transform.position = new Vector3(worldCenterX, worldCenterY, -10);

                // Render only target tilemap
                var renderers = Object.FindObjectsByType<TilemapRenderer>(FindObjectsSortMode.None);
                foreach (var r in renderers) r.enabled = false;
                var targetRenderer = sourceTilemap.GetComponent<TilemapRenderer>();
                if (targetRenderer) targetRenderer.enabled = true;

                cam.Render();

                foreach (var r in renderers) r.enabled = true;

                // Save texture
                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(chunkWidthPx, chunkHeightPx, TextureFormat.RGBA32, false);
                tex.ReadPixels(new Rect(0, 0, chunkWidthPx, chunkHeightPx), 0, 0);
                tex.Apply();

                byte[] png = tex.EncodeToPNG();
                string texPath = Path.Combine(prefabFolder, $"chunk_{x}_{y}.png");
                File.WriteAllBytes(texPath, png);

                string assetPath = texPath.Replace(Application.dataPath, "Assets");
                AssetDatabase.ImportAsset(assetPath);
                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = pixelsPerUnit;
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                bakedSprites.Add(sprite);

                RenderTexture.active = null;
                rt.Release();
                Object.DestroyImmediate(rt);
                Object.DestroyImmediate(tex);
            }
        }


        Object.DestroyImmediate(cam.gameObject);

        // Create assembled prefab
        GameObject mapParent = new GameObject(prefabName);
        int index = 0;
        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                if (index >= bakedSprites.Count) break;
                Sprite sprite = bakedSprites[index++];
                GameObject chunkObj = new GameObject($"Chunk_{x}_{y}");
                chunkObj.transform.SetParent(mapParent.transform);

                var sr = chunkObj.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingOrder = 0;
                sr.drawMode = SpriteDrawMode.Simple;

                // Position correctly
                float offsetX = x * chunkPixelSize / (float)pixelsPerUnit;
                float offsetY = y * chunkPixelSize / (float)pixelsPerUnit;
                chunkObj.transform.position = new Vector3(offsetX, offsetY, 0);
            }
        }

        string prefabPath = Path.Combine(outputFolder, prefabName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(mapParent, prefabPath);
        Object.DestroyImmediate(mapParent);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Done!", $"Tilemap baked to prefab:\n{prefabPath}", "OK");
        Debug.Log($"✅ Tilemap baked successfully to: {prefabPath}");
    }
}
