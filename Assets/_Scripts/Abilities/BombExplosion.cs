using UnityEngine;

/// <remarks>
/// It uses data from an <see cref="InstantiatedAbilityHandler"/> to apply damage and knockback.
/// </remarks>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BombExplosion : InstantiatedAbilityMono
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Initializes the explosion instance with a lifetime and ability configuration.
    /// </summary>
    public void Init(float timer, InstantiatedAbilityHandler ability)
    {
        this.timer = timer;
        Ability = ability;
    }

    protected override void FixedUpdate()
    {
        CountDownActiveTimer(Time.fixedDeltaTime);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: Ability.GetStat("Damage") * 2,
            knockbackForce: Ability.GetStat("KnockbackForce"),
            knockbackDuration: Ability.KnockbackDuration
        );
    }
}
