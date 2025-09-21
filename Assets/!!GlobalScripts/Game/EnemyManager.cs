using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance { get; private set; }
    [SerializeField]
    private Enemy _prefab;
    public GameObject target;
    private float delay = 1f;
    public List<Enemy> enemies { get; private set; }
    void Start()
    {
        _instance = this;
        enemies = new List<Enemy>();
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

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(_prefab, new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, 0), Quaternion.identity);
        enemy.transform.parent = transform;
        enemy.SetTarget(target.transform);
        enemy.gameObject.SetActive(true);
        enemies.Add(enemy);
    }
}
