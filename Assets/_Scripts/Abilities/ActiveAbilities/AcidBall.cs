using UnityEngine;

public class AcidBall : Fireball
{
    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Damage, GetType(), KnockbackForce, Ability.KnockbackDuration);
        Acid acidEffect = new(GameData.player, enemy, damage: Damage * 0.1f, percentSlow: -0.5f);
        enemy.Statuses.ApplyEffect(acidEffect);
        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
