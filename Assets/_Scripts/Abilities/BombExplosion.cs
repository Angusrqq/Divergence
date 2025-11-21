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
    /// <summary>
    /// Initializes the explosion instance with a lifetime and ability configuration.
    /// </summary>
    /// <param name="timer">Lifetime in seconds before the explosion despawns.</param>
    /// <param name="ability">Ability data that provides damage and knockback values.</param>
    public void Init(float timer, InstantiatedAbilityHandler ability)
    {
        this.timer = timer;
        Ability = ability;
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        CountDownActiveTimer(Time.fixedDeltaTime);
    }

    /// <summary>
    /// Applies damage and knockback to enemies that enter the explosion's trigger area.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Damage * 2, knockbackForce: KnockbackForce, knockbackDuration: Ability.KnockbackDuration);
    }
}
