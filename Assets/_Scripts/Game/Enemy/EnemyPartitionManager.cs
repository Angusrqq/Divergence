using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// <para>
/// <c>EnemyManager</c> is a singleton class that manages the spawning and tracking of enemies in the game.
/// </para>
/// </summary>
public class EnemyPartitionManager : MonoBehaviour
{
    public static EnemyPartitionManager _instance { get; private set; }
    [SerializeField] private PartitionEnemy _prefab;
    public MonoBehaviour target;
    private float delay = 1f;
    public static List<PartitionEnemy> Enemies { get; private set; }

    /// <summary>
    /// Initializes the enemy manager singleton and creates a new list for storing enemies.
    /// </summary>
    void Start()
    {
        _instance = this;
        Enemies = new List<PartitionEnemy>();
    }

    /// <summary>
    /// Updates the enemy spawning timer. If the timer is up, an enemy is spawned and the timer is reset.
    /// </summary>
    void Update()
    {
        if (delay - Time.deltaTime <= 0)
        {
            SpawnEnemy();
            delay = 0.02f;
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    /// <summary>
    /// <para>
    /// <c>SpawnEnemy</c> method spawns an enemy at a random position within a defined range and sets its target to the player.
    /// </para>
    /// </summary>
    private void SpawnEnemy()
    {
        PartitionEnemy enemy = Instantiate(_prefab, new Vector3(Random.Range(-2f, 2f), 0, 0), Quaternion.identity);

        enemy.transform.parent = transform;
        enemy.Init(GameData.Enemies[Random.Range(0, GameData.Enemies.Count)], target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
    }
}
