using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : InstantiatedAbilityMono
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        GetTargetForProjectile(Ability, out Target);
        if (Target != null)
        {
            direction = (Target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            Destroy(gameObject);
            Ability.StartCooldown();
            return;
        }

        base.Start();
    }

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: Ability.GetStat("Damage"),
            type: GetType(),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration,
            useSound: false
        );

        Burn burnEffect = new(GameData.player, enemy, damage: Ability.GetStat("Damage") * 0.1f);
        enemy.Statuses.ApplyEffect(burnEffect);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
