using System.Runtime.CompilerServices;
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
            
            base.Start();
        }
        else
        {
            Destroy(gameObject);
            Ability.StartCooldown();
            return;
        }
    }

    public override void EnemyCollision(Enemy enemy)
    {
        Stat abilityDamage = Ability.GetStat("Damage");

        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: abilityDamage,
            type: GetType(),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration,
            useSound: false
        );

        Burn burnEffect = new(GameData.player, enemy, damage: abilityDamage * 0.1f);
        enemy.Statuses.ApplyEffect(burnEffect);

        Destroy(gameObject);
        Ability.StartCooldown();
    }
}
