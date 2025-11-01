using UnityEngine;

public class AcidBall : Fireball
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(GameData.player.gameObject, Ability.damage, GetType(), Ability.KnockbackForce, Ability.KnockbackDuration, useSound: false);
            Acid acidEffect = new(GameData.player, enemy, damage: Ability.damage * 0.1f, percentSlow: -0.5f);
            enemy.Statuses.ApplyEffect(acidEffect);
            Destroy(gameObject);
            Ability.StartCooldown();
        }
    }
}
