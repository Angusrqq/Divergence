using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// <para>
/// <c>EnemyManager</c> is a singleton class that manages the spawning and tracking of enemies in the game.
/// </para>
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance { get; private set; }
    [SerializeField] private Enemy _prefab;
    public GameObject target;
    private float delay = 1f;
    public List<Enemy> Enemies { get; private set; }

    void Start()
    {
        _instance = this;
        Enemies = new List<Enemy>();
        //SpawnEnemy();
    }

    void Update()
    {
        if (delay - Time.deltaTime <= 0)
        {
            SpawnEnemy();
            delay = 1f;
        }
        else delay -= Time.deltaTime;
    }

    /// <summary>
    /// <para>
    /// <c>SpawnEnemy</c> method spawns an enemy at a random position within a defined range and sets its target to the player.
    /// </para>
    /// </summary>
    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(_prefab, new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, 0), Quaternion.identity);

        enemy.transform.parent = transform;
        enemy.SetTarget(target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
    }
}
