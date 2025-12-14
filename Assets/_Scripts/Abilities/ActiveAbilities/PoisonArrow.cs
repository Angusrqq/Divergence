public class PoisonArrow : Fireball
{
    public override void EnemyCollision(Enemy enemy)
    {
        Stat abilityDamage = Ability.GetStat("Damage");

        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: abilityDamage,
            type: GetType(),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration
        );
        
        Acid acidEffect = new(
            sender: GameData.player,
            target: enemy,
            damage: abilityDamage * 0.1f,
            percentSlow: -0.5f
        );
        enemy.Statuses.ApplyEffect(acidEffect);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
