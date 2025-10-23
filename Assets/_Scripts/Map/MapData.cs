using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tilemap/MapData", fileName = "NewMapData")]
public class MapData : ScriptableObject
{
    public string mapName;
    public Vector2Int size;
    public Vector2Int startPosition;
    public List<TileInfo> tiles = new();
    public Sprite icon;

    [System.Serializable]
    public class TileInfo
    {
        public Vector3Int position;
        public TileBase tile;
    }

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

    public void ApplyToTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
        foreach (var t in tiles)
        {
            tilemap.SetTile(t.position, t.tile);
        }
    }
}
