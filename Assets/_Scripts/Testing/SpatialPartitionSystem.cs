using System.Collections.Generic;
using UnityEngine;

public class SpatialGridSystem : MonoBehaviour
{
    public static SpatialGridSystem Instance { get; private set; }

    public float cellSize = 2f;
    public FastSpatialGrid2D<GridObject> Grid { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Grid = new FastSpatialGrid2D<GridObject>(cellSize);
    }
    // void OnDrawGizmos()
    // {
    //     if (Grid == null) return;
    //     if (Camera.main == null) return;

    //     Vector3 focusPoint = Camera.main.transform.position;
    //     float radius = 10f;

    //     Grid.DrawGridCellsAroundPointGizmos(focusPoint, radius, Color.magenta);
    // }

}

