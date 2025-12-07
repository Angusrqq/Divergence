using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shuriken : InstantiatedAbilityMono
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        Enemy _target = FindClosest();

        if (_target != null)
        {
            direction = (FindClosest().transform.position - transform.position).normalized;
        }
        else
        {
            Destroy(gameObject);
            Ability.StartCooldown();
            return;
        }
        base.Start();
    }

    protected override void FixedUpdateLogic()
    {
        transform.RotateAround(transform.position, Vector3.forward, 200);
        rb.MovePosition(Ability.Speed * direction + rb.position);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"), GetType(), Ability.GetStat("Knockback Force"), Ability.KnockbackDuration);
        
        Destroy(gameObject);
        Ability.StartCooldown();
    }
}