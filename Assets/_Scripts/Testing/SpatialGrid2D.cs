using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid2D<T>
{
    private readonly float cellSize;
    private readonly Dictionary<Vector2Int, HashSet<T>> grid = new();
    private readonly Dictionary<T, Vector2Int> objectCellMap = new();

    public SpatialGrid2D(float cellSize)
    {
        this.cellSize = cellSize;
    }

    private Vector2Int GetCellCoords(Vector2 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.y / cellSize)
        );
    }

    public void Register(T obj, Vector2 position)
    {
        Vector2Int cell = GetCellCoords(position);
        if (!grid.TryGetValue(cell, out var list))
        {
            list = new HashSet<T>();
            grid[cell] = list;
        }
        list.Add(obj);
        objectCellMap[obj] = cell;
    }

    public void Unregister(T obj)
    {
        if (objectCellMap.TryGetValue(obj, out var cell))
        {
            if (grid.TryGetValue(cell, out var list))
                list.Remove(obj);
            objectCellMap.Remove(obj);
        }
    }

    public void UpdateObjectPosition(T obj, Vector2 position)
    {
        Vector2Int newCell = GetCellCoords(position);
        if (objectCellMap.TryGetValue(obj, out var oldCell))
        {
            if (oldCell == newCell) return;
            Unregister(obj);
        }
        Register(obj, position);
    }

    public List<T> GetNearby(Vector2 position)
    {
        Vector2Int cell = GetCellCoords(position);
        List<T> nearby = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighbor = new(cell.x + x, cell.y + y);
                if (grid.TryGetValue(neighbor, out var list))
                    nearby.AddRange(list);
            }
        }
        return nearby;
    }

    public void DrawGridCellsAroundPointGizmos(Vector2 center, float radius, Color color)
    {
        if (cellSize <= 0) return;

        int cellRange = Mathf.CeilToInt(radius / cellSize);
        Vector2Int centerCell = new Vector2Int(
            Mathf.FloorToInt(center.x / cellSize),
            Mathf.FloorToInt(center.y / cellSize)
        );

        Gizmos.color = color;

        for (int x = -cellRange; x <= cellRange; x++)
        {
            for (int y = -cellRange; y <= cellRange; y++)
            {
                Vector2Int cell = new Vector2Int(centerCell.x + x, centerCell.y + y);
                Vector3 bottomLeft = new Vector3(cell.x * cellSize, cell.y * cellSize, 0);
                Vector3 centerPos = bottomLeft + new Vector3(cellSize / 2f, cellSize / 2f, 0);

                // Skip cells outside circular radius for clarity
                if (Vector2.Distance(center, centerPos) > radius)
                    continue;

                // Draw square border
                Vector3 size = new Vector3(cellSize, cellSize, 0);
                Gizmos.DrawWireCube(centerPos, size);

                // Optionally fill occupied cells slightly
                if (HasObjectsInCell(cell))
                {
                    Color fill = new Color(color.r, color.g, color.b, 0.15f);
                    Gizmos.color = fill;
                    Gizmos.DrawCube(centerPos, size * 0.95f);
                    Gizmos.color = color; // reset for next outline
                }
            }
        }
    }
    public bool HasObjectsInCell(Vector2Int cell)
    {
        return grid.ContainsKey(cell) && grid[cell].Count > 0;
    }
}
