using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FastSpatialGrid2D<T>
{
    private readonly float cellSize, invCellSize;
    private readonly Dictionary<(int,int), List<T>> grid = new();
    private readonly Dictionary<T, (int,int)> cellMap = new();

    public FastSpatialGrid2D(float size)
    {
        cellSize = size;
        invCellSize = 1f / size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (int,int) GetCellCoords(Vector2 pos)
    {
        int x = (int)Mathf.Floor(pos.x * invCellSize);
        int y = (int)Mathf.Floor(pos.y * invCellSize);
        return (x, y);
    }

    public void Register(T obj, Vector2 pos)
    {
        var cell = GetCellCoords(pos);
        if (!grid.TryGetValue(cell, out var list))
            grid[cell] = list = new List<T>(8);
        list.Add(obj);
        cellMap[obj] = cell;
    }

    public void UpdateObjectPosition(T obj, Vector2 pos)
    {
        var newCell = GetCellCoords(pos);
        if (cellMap.TryGetValue(obj, out var oldCell) && oldCell.Equals(newCell))
            return;
        Unregister(obj);
        Register(obj, pos);
    }

    public void Unregister(T obj)
    {
        if (cellMap.TryGetValue(obj, out var cell))
        {
            if (grid.TryGetValue(cell, out var list))
            {
                int i = list.IndexOf(obj);
                if (i >= 0) list[i] = list[^1];
                list.RemoveAt(list.Count - 1);
            }
            cellMap.Remove(obj);
        }
    }

    public void GetNearby(Vector2 pos, List<T> buffer)
    {
        buffer.Clear();
        var (cx, cy) = GetCellCoords(pos);

        for (int x = -1; x <= 1; x++)
        for (int y = -1; y <= 1; y++)
            if (grid.TryGetValue((cx + x, cy + y), out var list))
                buffer.AddRange(list);
    }
}
