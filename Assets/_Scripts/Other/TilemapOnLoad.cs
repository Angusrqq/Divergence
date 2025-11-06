using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Initialization component that registers the attached Tilemap to the global
/// <see cref="GameData"/> when the scene loads, making it accessible to map loading systems.
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class TilemapOnLoad : MonoBehaviour
{
    /// <summary>
    /// Stores a reference to this GameObject's Tilemap in <see cref="GameData.TilemapToLoadMaps"/>.
    /// </summary>
    void Awake()
    {
        GameData.TilemapToLoadMaps = GetComponent<Tilemap>();
    }
}
