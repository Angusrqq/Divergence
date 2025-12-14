public class RiceBall : InstantiatedAbilityMono
{
    private Enemy target;

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

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: Ability.GetStat("Damage"),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration
        );

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
