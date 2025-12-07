
using UnityEngine;

/// <summary>
/// This class represents a test ability that targets the nearest enemy.
/// It inherits from InstantiatedAbilityMono, which suggests it's a component
/// that represents an instantiated ability in the game world.
/// </summary>
public class RiceBall : InstantiatedAbilityMono
{
    private Enemy target;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Finds the closest enemy and destroys this ability if no enemy is found.
    /// </summary>
    protected override void Start()
    {
        target = FindClosest();

        if (target == null)
        {
            Destroy(gameObject);
            Ability.StartCooldown();
            return;
        }
        base.Start();
    }

    protected override void FixedUpdateLogic()
    {
        // Update direction towards target if target exists, otherwise maintain current direction
        direction = target ? (target.transform.position - transform.position).normalized : direction.normalized;
        
        rb.MovePosition(Ability.Speed * direction + rb.position);
    }

    /// <summary>
    /// Called when this ability collides with another object.
    /// Checks if the collided object is an enemy and applies damage if true.
    /// </summary>
    /// <param name="other">The collider of the object this ability collided with.</param>
    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"), knockbackForce: Ability.GetStat("Knockback Force"), knockbackDuration: Ability.KnockbackDuration);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
