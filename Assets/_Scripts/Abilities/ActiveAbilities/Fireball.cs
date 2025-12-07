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
        enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"), GetType(), Ability.GetStat("Knockback Force"), Ability.KnockbackDuration, useSound: false);
        Burn burnEffect = new(GameData.player, enemy, damage: Ability.GetStat("Damage") * 0.1f);
        enemy.Statuses.ApplyEffect(burnEffect);
        Destroy(gameObject);
        Ability.StartCooldown();
    }

    public override void Upgrade(InstantiatedAbilityHandler ability)
    {
        ability.damage *= 1.05f;
        ability.localProjectilesAmount += 1;
    }
}
