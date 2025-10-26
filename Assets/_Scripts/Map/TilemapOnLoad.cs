using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapOnLoad : MonoBehaviour
{
    void Awake()
    {
        GameData.TilemapToLoadMaps = GetComponent<Tilemap>();
    }
}
