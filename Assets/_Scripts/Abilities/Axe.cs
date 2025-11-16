using Unity.Mathematics;
using UnityEngine;

public class Axe : InstantiatedAbilityMono
{
    private float _forceTimer;
    private Vector2 _intialDirection;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
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
        }

        _intialDirection = direction;
    }

    protected override void FixedUpdateLogic()
    {
        _forceTimer -= Time.fixedDeltaTime;
        direction = Vector2.LerpUnclamped(_intialDirection, -_intialDirection, (Ability.ActiveTime / 2) - _forceTimer);
        transform.RotateAround(transform.position, Vector3.forward, 10);
        rb.MovePosition(Ability.Speed * direction + rb.position);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(GameData.player.gameObject, Ability.damage * (math.abs(direction.x) + math.abs(direction.y)), GetType(), Ability.KnockbackForce, Ability.KnockbackDuration);
            Debug.Log($"Damage given: {Ability.damage * (math.abs(direction.x) + math.abs(direction.y))}");  
        }
    }
}
