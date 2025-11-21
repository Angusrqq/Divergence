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

    public Stat SpawnDelay = 1f;
    public MonoBehaviour Target;
    public int maxEnemyCount = 10000;
    
    private Camera _camera;
    private float delay;

    public static EnemyManager Instance { get; private set; }
    public static List<Enemy> Enemies { get; private set; }

    /// <summary>
    /// Initializes the enemy manager singleton and creates a new list for storing enemies.
    /// </summary>
    void Awake()
    {
        _camera = Camera.main;
        delay = SpawnDelay;
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
            bool vertical = Random.Range(0, 2) == 1;
            // Vector2 offset = new(10f, 10f);
            //TODO: apply an offset to this shit, dk how to explain (enemies spawn right at the edge, their sprite size is not included here)
            Vector2 viewportCoords = vertical ? new(Random.Range(0, 2), Random.Range(0f, 1f)) : new(Random.Range(0f, 1f), Random.Range(0, 2));
            Vector2 spawnPos = _camera.ViewportToWorldPoint(viewportCoords);
            SpawnEnemy(spawnPos, Quaternion.identity, Random.Range(0, GameData.Enemies.Count));
            delay = SpawnDelay / (1f + (GameData.GameTimerInstance.currentTime / 60f));
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    /// <summary>
    /// <para>
    /// <c>SpawnEnemy</c> method spawns an enemy at a Vector2 <c>pos</c> and sets its target to the player.
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
