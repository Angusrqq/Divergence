using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject asset containing tilemap data for serialization and runtime loading.
/// </summary>
[CreateAssetMenu(menuName = "Tilemap/MapData", fileName = "NewMapData")]
public class MapData : BaseScriptableObjectInfo
{
    [Header("Map Data")]
    public Vector2Int size;
    public Vector2Int startPosition;

    [Header("Tiles")]
    public List<TileInfo> tiles = new();

    [System.Serializable]
    public class TileInfo
    {
        public Vector3Int position;
        public TileBase tile;
    }

    /// <summary>
    /// Captures all tiles from the specified Tilemap and stores them in the MapData.
    /// This method first clears the existing tile list, then iterates through all positions in the Tilemap,
    /// adding each non-null tile to the list. Finally, it updates the MapData size to match the Tilemap bounds.
    /// </summary>
    /// <param name="tilemap">The Tilemap from which the tiles are captured.</param>
    public void CaptureFromTilemap(Tilemap tilemap)
    {
        tiles.Clear();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            var tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                tiles.Add(new TileInfo { position = pos, tile = tile });
            }
        }

        size = new Vector2Int(tilemap.cellBounds.size.x, tilemap.cellBounds.size.y);
    }

    /// <summary>
    /// Applies all tiles from the MapData to the specified Tilemap.
    /// </summary>
    /// <param name="tilemap">The Tilemap to which the tiles will be applied.</param>
    public void ApplyToTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
        foreach (var t in tiles)
        {
            tilemap.SetTile(t.position, t.tile);
        }
    }
}
