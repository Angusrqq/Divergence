using UnityEngine;

public class AcidBall : Fireball
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(GameData.player.gameObject, ability.damage, GetType(), ability.KnockbackForce, ability.KnockbackDuration, useSound: false);
            Acid acidEffect = new(GameData.player, enemy, damage: ability.damage * 0.1f, percentSlow: -0.5f);
            enemy.Statuses.ApplyEffect(acidEffect);
            Destroy(gameObject);
            ability.StartCooldown();
        }
    }
}
