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
        _forceTimer = Ability.ActiveTime / 1.8f;

        Enemy _target = FindClosest();
        if (_target != null)
        {
            direction = (_target.transform.position - transform.position).normalized;
            
            _intialDirection = direction;
            base.Start();
        }
        else
        {
            Destroy(gameObject);
            Ability.StartCooldown();

            return;
        }
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
