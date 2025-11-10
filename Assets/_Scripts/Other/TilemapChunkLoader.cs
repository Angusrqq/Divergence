using UnityEngine;
using System.Collections.Generic;

public class TilemapChunkLoader : MonoBehaviour
{
    [Header("Chunk Loading Settings")]
    public Transform player;
    public int chunkSize = 64;
    public int viewDistanceInChunks = 2;
    public string chunkFolder = "MapChunks"; // inside Resources folder!

    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int currentChunk;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("TilemapChunkLoader: Player reference not set!");
            enabled = false;
            return;
        }
        UpdateVisibleChunks(force: true);
    }

    void Update()
    {
        Vector2Int newChunk = GetPlayerChunk();
        if (newChunk != currentChunk)
        {
            currentChunk = newChunk;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks(bool force = false)
    {
        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();

        for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
        {
            for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
            {
                Vector2Int chunkCoord = currentChunk + new Vector2Int(x, y);
                chunksToKeep.Add(chunkCoord);

                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                }
            }
        }

        // Unload far chunks
        var keys = new List<Vector2Int>(loadedChunks.Keys);
        foreach (var coord in keys)
        {
            if (!chunksToKeep.Contains(coord))
            {
                UnloadChunk(coord);
            }
        }
    }

    void LoadChunk(Vector2Int coord)
    {
        string path = $"{chunkFolder}/Chunk_{coord.x}_{coord.y}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, transform);
            loadedChunks[coord] = instance;
        }
        else
        {
            Debug.LogWarning($"Chunk prefab not found: {path}");
        }
    }

    void UnloadChunk(Vector2Int coord)
    {
        if (loadedChunks.TryGetValue(coord, out GameObject go))
        {
            Destroy(go);
            loadedChunks.Remove(coord);
        }
    }

    Vector2Int GetPlayerChunk()
    {
        int cx = Mathf.FloorToInt(player.position.x / chunkSize);
        int cy = Mathf.FloorToInt(player.position.y / chunkSize);
        return new Vector2Int(cx, cy);
    }
}
