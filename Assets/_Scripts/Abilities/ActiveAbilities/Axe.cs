using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Axe : InstantiatedAbilityMono
{
    private float _forceTimer;
    private Vector2 _intialDirection;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        Enemy _target = FindClosest();
        _forceTimer = Ability.ActiveTime / 1.8f;

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

        _intialDirection = direction;
        base.Start();
    }

    protected override void FixedUpdateLogic()
    {
        _forceTimer -= Time.fixedDeltaTime;
        direction = Vector2.LerpUnclamped(_intialDirection, -_intialDirection, (Ability.ActiveTime / 2) - _forceTimer);

        transform.RotateAround(transform.position, Vector3.forward, 10);
        rb.MovePosition(Ability.Speed * direction + rb.position);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            source: GameData.player.gameObject,
            amount: Ability.GetStat("Damage"),
            type: GetType(),
            knockbackForce: Ability.GetStat("Knockback Force"),
            knockbackDuration: Ability.KnockbackDuration
        );
    }
}
