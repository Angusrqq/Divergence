using UnityEngine;

public class Shuriken : InstantiatedAbilityMono
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
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
        }
    }

    protected override void FixedUpdateLogic()
    {
        transform.RotateAround(transform.position, Vector3.forward, 200);
        rb.MovePosition(Ability.Speed * direction + rb.position);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(GameData.player.gameObject, Ability.damage, GetType(), Ability.KnockbackForce, Ability.KnockbackDuration);
            Destroy(gameObject);
            Ability.StartCooldown();
        }
    }
}