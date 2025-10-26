using System.Linq;
using UnityEngine;

// Uncomment the following lines if you want to use `FindClosestOnCrack` method
// using Unity.Collections;
// using Unity.Jobs;
// using Unity.Burst;

/// <summary>
/// This class represents a test ability that targets the nearest enemy.
/// It inherits from InstantiatedAbilityMono, which suggests it's a component
/// that represents an instantiated ability in the game world.
/// </summary>
public class NearestEnemyTest : InstantiatedAbilityMono
{
    private Enemy target;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Finds the closest enemy and destroys this ability if no enemy is found.
    /// </summary>
    public void Start()
    {
        target = FindClosest();

        if (target == null)
        {
            Destroy(gameObject);
            ability.StartCooldown();
        }
    }

    protected override void FixedUpdateLogic()
    {
        // Update direction towards target if target exists, otherwise maintain current direction
        direction = target ? (target.transform.position - transform.position).normalized : direction.normalized;
        // Move the rigidbody towards the target with the ability's speed
        rb.MovePosition(ability.speed * direction + rb.position);
    }

    /// <summary>
    /// Called when this ability collides with another object.
    /// Checks if the collided object is an enemy and applies damage if true.
    /// </summary>
    /// <param name="other">The collider of the object this ability collided with.</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            // Apply damage, knockback, and other effects to the enemy
            enemy.TakeDamage(gameObject, ability.damage, knockbackForce: ability.KnockbackForce, knockbackDuration: ability.KnockbackDuration);

            Destroy(gameObject);
            ability.StartCooldown();
        }
    }

    public Enemy FindClosest()
    {
        return EnemyManager.Enemies.OrderBy(enemy => Vector2.Distance(enemy.transform.position, GameData.player.transform.position)).FirstOrDefault(enemy => enemy != null);
    }

    // public struct FindClosestEnemy : IJob
    // {
    //     public float distance;
    //     NativeArray<int> enemy;
    //     public void Execute()
    //     {

    //     }
    // }

    // public Enemy FindClosestOnCrack()
    // {
    //     var enemies = EnemyManager.Enemies;
    //     if (enemies == null || enemies.Count == 0 || GameData.player == null)
    //         return null;

    //     int enemyCount = enemies.Count;
    //     Vector2 playerPos = GameData.player.transform.position;

    //     NativeArray<Vector2> enemyPositions = new NativeArray<Vector2>(enemyCount, Allocator.TempJob);
    //     NativeArray<float> distances = new NativeArray<float>(enemyCount, Allocator.TempJob);

    //     try
    //     {
    //         for (int i = 0; i < enemyCount; i++)
    //         {
    //             if (enemies[i] == null)
    //             {
    //                 enemyPositions[i] = Vector2.positiveInfinity;
    //                 continue;
    //             }

    //             enemyPositions[i] = enemies[i].transform.position;
    //         }

    //         var job = new DistanceJob
    //         {
    //             playerPosition = playerPos,
    //             enemyPositions = enemyPositions,
    //             distances = distances
    //         };

    //         JobHandle handle = job.Schedule(enemyCount, 64);
    //         handle.Complete();

    //         float minDist = float.MaxValue;
    //         int minIndex = -1;

    //         for (int i = 0; i < enemyCount; i++)
    //         {
    //             float dist = distances[i];
    //             if (dist < minDist)
    //             {
    //                 minDist = dist;
    //                 minIndex = i;
    //             }
    //         }

    //         if (minIndex < 0 || minIndex >= enemies.Count)
    //             return null;

    //         return enemies[minIndex];
    //     }
    //     finally
    //     {
    //         if (enemyPositions.IsCreated)
    //             enemyPositions.Dispose();
    //         if (distances.IsCreated)
    //             distances.Dispose();
    //     }
    // }

    // [BurstCompile]
    // private struct DistanceJob : IJobParallelFor
    // {
    //     [ReadOnly] public Vector2 playerPosition;
    //     [ReadOnly] public NativeArray<Vector2> enemyPositions;
    //     [WriteOnly] public NativeArray<float> distances;

    //     public void Execute(int index)
    //     {
    //         Vector2 enemyPos = enemyPositions[index];
    //         if (float.IsInfinity(enemyPos.x) || float.IsInfinity(enemyPos.y))
    //         {
    //             distances[index] = float.MaxValue;
    //             return;
    //         }

    //         float sqrDist = (enemyPos - playerPosition).sqrMagnitude;
    //         distances[index] = sqrDist;
    //     }
    // }
}
