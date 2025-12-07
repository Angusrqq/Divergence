public class PoisonArrow : Fireball
{
    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"), GetType(), Ability.GetStat("Knockback Force"), Ability.KnockbackDuration);
        
        Acid acidEffect = new(GameData.player, enemy, damage: Ability.GetStat("Damage") * 0.1f, percentSlow: -0.5f);
        enemy.Statuses.ApplyEffect(acidEffect);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
