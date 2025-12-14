using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArcanePulse : InstantiatedAbilityMono
{
    private readonly HashSet<Enemy> _enemiesInside = new();
    private readonly List<Enemy> _damageBuffer = new();
    private float _damageTimer;

    protected override void Start()
    {
        _damageTimer = Ability.KnockbackDuration;
        base.Start();
    }

    protected override void FixedUpdateLogic()
    {
        rb.position = GameData.player.transform.position;

        _damageTimer -= Time.fixedDeltaTime;

        if (_damageTimer <= 0)
        {
            _damageBuffer.Clear();
            _damageBuffer.AddRange(_enemiesInside);

            foreach (var enemy in _damageBuffer)
            {
                enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"), GetType());
            }

            _damageTimer = Ability.KnockbackDuration;
        }

        transform.RotateAround(transform.position, Vector3.forward, 2);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        _enemiesInside.Add(enemy);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemiesInside.Remove(enemy);
        }
    }
}
