public class PoisonArrow : Fireball
{
    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: Ability.GetStat("Damage"),
            type: GetType(),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration
        );
        
        Acid acidEffect = new(GameData.player, enemy, damage: Ability.GetStat("Damage") * 0.1f, percentSlow: -0.5f);
        enemy.Statuses.ApplyEffect(acidEffect);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
