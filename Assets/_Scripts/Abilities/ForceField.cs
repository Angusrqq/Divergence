using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ForceField : InstantiatedAbilityMono
{
    private List<Enemy> _enemiesInside = new();
    private List<Enemy> _enemiesToAdd = new();
    private List<Enemy> _enemiesToRemove = new();
    private float _damageTimer;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        _damageTimer = Ability.KnockbackDuration;
        base.Start();
    }

    protected override void FixedUpdateLogic()
    {
        rb.position = GameData.player.transform.position;
        if (_damageTimer > 0)
        {
            _damageTimer -= Time.fixedDeltaTime;
        }
        
        foreach (var enemy in _enemiesToAdd)
        {
            _enemiesInside.Add(enemy);
        }
        _enemiesToAdd.Clear();

        foreach (var enemy in _enemiesToRemove)
        {
            _enemiesInside.Remove(enemy);
        }
        _enemiesToRemove.Clear();

        if (_damageTimer <= 0)
        {
            foreach (var enemy in _enemiesInside)
            {
                enemy.TakeDamage(GameData.player.gameObject, Damage, GetType());
            }
            _damageTimer = Ability.KnockbackDuration;
        }
        transform.RotateAround(transform.position, Vector3.forward, 2);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        if (!_enemiesInside.Contains(enemy) && !_enemiesToAdd.Contains(enemy))
        {
            _enemiesToAdd.Add(enemy);
        }
        enemy.TakeDamage(GameData.player.gameObject, Damage, GetType());
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (_enemiesInside.Contains(enemy) && !_enemiesToRemove.Contains(enemy))
            {
                _enemiesToRemove.Add(enemy);
            }
            if (_enemiesToAdd.Contains(enemy))
            {
                _enemiesToAdd.Remove(enemy);
            }
        }
    }
}
