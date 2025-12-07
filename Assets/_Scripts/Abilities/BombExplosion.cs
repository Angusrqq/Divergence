using UnityEngine;

/// <summary>
/// Handles the behavior of a bomb explosion instance spawned by an ability.
/// </summary>
/// <remarks>
/// This component relies on <see cref="Rigidbody2D"/> for physics timing and a trigger <see cref="Collider2D"/>
/// to detect damageable targets. It uses data from an <see cref="InstantiatedAbilityHandler"/> to
/// apply damage and knockback when overlapping enemies. Damage applied on hit is doubled relative to the
/// base ability damage.
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
    /// <param name="timer">Lifetime in seconds before the explosion despawns.</param>
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
