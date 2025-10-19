using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ActualPartitionGrid
{
    public static int HALF_WIDTH_STATIC;
    public const int WIDTH = 100;
    public const int HEIGHT = 100;
    public const int TOTAL_PARTITIONS = WIDTH * HEIGHT;
    public static int HALF_HEIGHT_STATIC;
    public static int CELL_WIDTH_STATIC;
    public static int CELL_HEIGHT_STATIC;
    public static int CELLS_PER_ROW_STATIC;

    public static int GetSpatialGroupStatic(float xPos, float yPos)
    {
        float adjustedX = xPos + HALF_WIDTH_STATIC;
        float adjustedY = yPos + HALF_WIDTH_STATIC;

        int xIndex = (int)(adjustedX / CELL_WIDTH_STATIC);
        int yIndex = (int)(adjustedY / CELL_HEIGHT_STATIC);

        return xIndex + yIndex * CELLS_PER_ROW_STATIC;
    }

    public static List<int> GetExpandedSpatialGroupsV2(int spatialGroup, int radius = 1)
    {
        List<int> expandedSpatialGroups = new();

        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int newGroup = spatialGroup + dx + dy * WIDTH;

                bool isWithinWidth = newGroup % WIDTH >= 0 && newGroup % WIDTH < WIDTH;
                bool isWithinHeight = newGroup % HEIGHT >= 0 && newGroup % HEIGHT < HEIGHT;
                bool isWithinBounds = isWithinWidth && isWithinHeight;

                bool isWithinPartitions = newGroup >= 0 && newGroup < TOTAL_PARTITIONS;

                if (isWithinBounds && isWithinPartitions)
                {
                    expandedSpatialGroups.Add(newGroup);
                }
            }
        }
        return expandedSpatialGroups.Distinct().ToList();
    }
    
    public static List<Enemy> GetAllEnemiesInSpatialGroups(List<int> spatialGroups)
    {
        List<Enemy> enemies = new();
        foreach(int spatialGroup in spatialGroups)
        {
            //enemies.AddRange(EnemyManager._instance.enemySpatialGroups[spatialGroup]);
        }
        return enemies;
    }
}
