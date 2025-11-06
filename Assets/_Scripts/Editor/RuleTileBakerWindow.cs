using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

/// <summary>
/// An EditorWindow for baking RuleTiles in a Tilemap to a new Tilemap.
/// </summary>
public class RuleTileBakerWindow : EditorWindow
{
    private Tilemap _sourceTilemap;

    [MenuItem("Tools/Tilemap/Bake RuleTiles")]
    public static void ShowWindow()
    {
        GetWindow<RuleTileBakerWindow>("Bake RuleTiles");
    }

    /// <summary>
    /// The main OnGUI method of the EditorWindow.
    /// It displays a label, a space, an ObjectField for the source Tilemap, a help box if the source Tilemap is null, and a button to bake the RuleTiles to a new Tilemap.
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("RuleTile Baker", EditorStyles.boldLabel);
        GUILayout.Space(5);

        _sourceTilemap = (Tilemap)EditorGUILayout.ObjectField("Source Tilemap", _sourceTilemap, typeof(Tilemap), true);

        if (_sourceTilemap == null)
        {
            EditorGUILayout.HelpBox("Select a Tilemap that contains RuleTiles", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Bake to New Tilemap"))
        {
            BakeRuleTiles();
        }
    }

    /// <summary>
    /// Bakes all RuleTiles in the source Tilemap into a new Tilemap.
    /// </summary>
    /// <remarks>
    /// This function will create a new GameObject with a Tilemap and TilemapRenderer component.
    /// It will then copy all RuleTiles from the source Tilemap to the new Tilemap.
    /// The new Tilemap will be a child of the source Tilemap's Grid component.
    /// </remarks>
    private void BakeRuleTiles()
    {
        var grid = _sourceTilemap.GetComponentInParent<Grid>();
        if (grid == null)
        {
            Debug.LogError("Tilemap has no Grid component");
            return;
        }

        GameObject bakedObj = new GameObject(_sourceTilemap.name + "_Baked");
        bakedObj.transform.SetParent(grid.transform, false);
        var bakedTilemap = bakedObj.AddComponent<Tilemap>();
        bakedObj.AddComponent<TilemapRenderer>();

        Undo.RegisterCreatedObjectUndo(bakedObj, "Bake RuleTiles");

        BoundsInt bounds = _sourceTilemap.cellBounds;
        TileBase[] allTiles = _sourceTilemap.GetTilesBlock(bounds);

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
