using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class RuleTileBakerWindow : EditorWindow
{
    private Tilemap sourceTilemap;

    [MenuItem("Tools/Tilemap/Bake RuleTiles")]
    public static void ShowWindow()
    {
        GetWindow<RuleTileBakerWindow>("Bake RuleTiles");
    }

    private void OnGUI()
    {
        GUILayout.Label("RuleTile Baker", EditorStyles.boldLabel);
        GUILayout.Space(5);

        sourceTilemap = (Tilemap)EditorGUILayout.ObjectField("Source Tilemap", sourceTilemap, typeof(Tilemap), true);

        if (sourceTilemap == null)
        {
            EditorGUILayout.HelpBox("Select a Tilemap that contains RuleTiles", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Bake to New Tilemap"))
        {
            BakeRuleTiles();
        }
    }

    private void BakeRuleTiles()
    {
        var grid = sourceTilemap.GetComponentInParent<Grid>();
        if (grid == null)
        {
            Debug.LogError("Tilemap не находится в Grid!");
            return;
        }

        GameObject bakedObj = new GameObject(sourceTilemap.name + "_Baked");
        bakedObj.transform.SetParent(grid.transform, false);
        var bakedTilemap = bakedObj.AddComponent<Tilemap>();
        bakedObj.AddComponent<TilemapRenderer>();

        Undo.RegisterCreatedObjectUndo(bakedObj, "Bake RuleTiles");

        BoundsInt bounds = sourceTilemap.cellBounds;
        TileBase[] allTiles = sourceTilemap.GetTilesBlock(bounds);

        int total = bounds.size.x * bounds.size.y;
        int progress = 0;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int pos = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                    bakedTilemap.SetTile(pos, tile);
                }

                progress++;
                if (progress % 500 == 0)
                {
                    EditorUtility.DisplayProgressBar("Baking RuleTiles...", "Processing tiles", (float)progress / total);
                }
            }
        }

        bakedTilemap.RefreshAllTiles();
        EditorUtility.ClearProgressBar();

        Debug.Log("RuleTiles baked successfully to " + bakedObj.name);
        Selection.activeGameObject = bakedObj;
    }
}
