using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;

    public MonoBehaviour Target;
    public int maxEnemyCount = 10000;
    public Action<Enemy> OnEnemyDeath;

    private Camera _camera;
    private Coroutine _advanceCoroutine;
    private float _spawnCredits = 0f;
    private float _minKillRateToAdvance = 6f;
    private float _minKillsToAdvance = 2000f;
    private int _currentPhase = 1;
    private float _currentExperience = 1f;
    private int m_currentEnemyIndex = 0;

    public static EnemyManager Instance { get; private set; }
    public static List<Enemy> Enemies { get; private set; }
    public int CurrentPhase => _currentPhase;
    private int _currentEnemyIndex
    {
        get => m_currentEnemyIndex;
        set
        {
            if (value >= GameData.Enemies.Count)
            {
                m_currentEnemyIndex = 0;
            }
            else
            {
                m_currentEnemyIndex = value;
            }
        }
    }

    public void TriggerEnemyDeath(Enemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }

    void Awake()
    {
        _camera = Camera.main;
        Instance = this;
        Enemies = new List<Enemy>();
    }

    void Update()
    {
        if ((KillCounter.Kills >= _minKillsToAdvance || KillCounter.KillsPerSecond >= _minKillRateToAdvance) && _advanceCoroutine == null)
        {
            _advanceCoroutine = StartCoroutine(AdvanceCoroutine());
        }

        float rate = SpawnRate(GameData.GameTimerInstance.currentTime, KillCounter.KillsPerSecond);

        _spawnCredits += rate * Time.deltaTime;
        while(_spawnCredits >= _currentPhase && Enemies.Count < maxEnemyCount)
        {
            SpawnEnemy(_currentEnemyIndex, _currentPhase, _currentPhase * 0.4f);
            _spawnCredits -= _currentPhase;
        }
    }

    private static float SpawnRate(float time, float kps)
    {
        float baseRate = 0.3f;
        float timeScale = 1f + time * 0.03f;
        float kpsScale = 1f + kps * 0.01f;

        return baseRate * timeScale * kpsScale;
    }

    private void Advance()
    {
        // Multiply existing values of minKillsToAdvance and minKillRateToAdvance by some factor
        _currentPhase++;

        _minKillsToAdvance *= 0.4f + _currentPhase;
        _minKillRateToAdvance *= 0.4f + _currentPhase;

        _currentEnemyIndex++;
        _currentExperience *= 2;
    }

    private IEnumerator AdvanceCoroutine()
    {
        int nextPhase = _currentPhase + 1;
        int count = 1;
        int enemies = 1;
        int nextEnemyIndex = _currentEnemyIndex + 1 >= GameData.Enemies.Count ? 0 : _currentEnemyIndex + 1;

        while(count <= 4)
        {
            for (int i = 0; i <= enemies; i++)
            {
                SpawnEnemy(
                    nextEnemyIndex,
                    maxHealthMult: nextPhase * (4 / count),
                    damageMult: nextPhase * (4 / count) * 0.4f
                );
            }

            count++;
            enemies *= 2;

            yield return new WaitForSeconds(5f);
        }

        Advance();
        _advanceCoroutine = null;
    }

    private void SpawnEnemy(Vector2 pos, Quaternion rot, int enemyIndex = 0)
    {
        Enemy enemy = Instantiate(_prefab, pos, rot);
        enemy.transform.parent = transform;

        enemy.Init(GameData.Enemies[enemyIndex], Target.transform);
        enemy.gameObject.SetActive(true);

        Enemies.Add(enemy);
    }

    private void SpawnEnemy(int enemyIndex)
    {
        bool vertical = UnityEngine.Random.Range(0, 2) == 1;
        // Vector2 offset = new(10f, 10f);

        //TODO: apply an offset to this shit, dk how to explain (enemies spawn right at the edge, their sprite size is not included here)

        float edge = UnityEngine.Random.Range(0, 2);
        float inside = UnityEngine.Random.Range(0f, 1f);
        Vector2 viewportCoords = vertical
            ? new(edge, inside)
            : new(inside, edge);

        Vector2 spawnPos = _camera.ViewportToWorldPoint(viewportCoords);
        SpawnEnemy(spawnPos, Quaternion.identity, enemyIndex);
    }

    private void SpawnEnemy(int enemyIndex, float maxHealthMult, float damageMult, float moveSpeedMult = 1f)
    {
        bool vertical = UnityEngine.Random.Range(0, 2) == 1;

        float edge = UnityEngine.Random.Range(0, 2);
        float inside = UnityEngine.Random.Range(0f, 1f);
        Vector2 viewportCoords = vertical
            ? new(edge, inside)
            : new(inside, edge);

        Vector2 spawnPos = _camera.ViewportToWorldPoint(viewportCoords);
        
        Enemy enemy = Instantiate(_prefab, spawnPos, Quaternion.identity);
        enemy.transform.parent = transform;

        EnemyData data = GameData.Enemies[enemyIndex];
        enemy.Init(
            data: data,
            newTarget: Target.transform,
            maxHealth: data.BaseMaxHealth * maxHealthMult,
            damage: data.Damage * damageMult,
            moveSpeed: data.BaseMovementSpeed * moveSpeedMult,
            experience: _currentExperience
        );
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
