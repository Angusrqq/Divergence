using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// <para>
/// <c>EnemyManager</c> is a singleton class that manages the spawning and tracking of enemies in the game.
/// </para>
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;

    public MonoBehaviour Target;
    public static EnemyManager Instance { get; private set; }
    public static List<Enemy> Enemies { get; private set; }

    private float delay = 1f;

    /// <summary>
    /// Initializes the enemy manager singleton and creates a new list for storing enemies.
    /// </summary>
    void Start()
    {
        Instance = this;
        Enemies = new List<Enemy>();
    }

    /// <summary>
    /// Updates the enemy spawning timer. If the timer is up, an enemy is spawned and the timer is reset.
    /// </summary>
    void Update()
    {
        if (delay - Time.deltaTime <= 0)
        {
            SpawnEnemy();
            delay = 0.5f;
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
        Enemy enemy = Instantiate(_prefab, new Vector3(Random.Range(-2f, 2f), 0, 0), Quaternion.identity);

        enemy.transform.parent = transform;
        enemy.Init(GameData.Enemies[Random.Range(0, GameData.Enemies.Count)], Target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
        
    }
}
