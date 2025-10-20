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
    public int maxEnemyCount = 10000;
    private Camera _camera;
    public static EnemyManager Instance { get; private set; }
    public static List<Enemy> Enemies { get; private set; }

    private float delay = 1f;

    /// <summary>
    /// Initializes the enemy manager singleton and creates a new list for storing enemies.
    /// </summary>
    void Awake()
    {
        _camera = Camera.main;
    }
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
            SpawnEnemy(new Vector2(Random.Range(-2f, 2f), 0), Quaternion.identity, Random.Range(0, GameData.Enemies.Count));
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
    private void SpawnEnemy(Vector2 pos, Quaternion rot, int enemyIndex = 0)
    {
        Enemy enemy = Instantiate(_prefab, pos, rot);

        enemy.transform.parent = transform;
        enemy.Init(GameData.Enemies[enemyIndex], Target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
    }
    
    private void SpawnEnemy(Vector2 pos, Quaternion rot, EnemyData enemyData)
    {
        Enemy enemy = Instantiate(_prefab, pos, rot);

        enemy.transform.parent = transform;
        enemy.Init(enemyData, Target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
    }
}
