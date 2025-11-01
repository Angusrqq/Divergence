using Unity.VisualScripting;
using UnityEngine;

public class Fireball : InstantiatedAbilityMono
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
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            Destroy(gameObject);
            Ability.StartCooldown();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(GameData.player.gameObject, Ability.damage, GetType(), Ability.KnockbackForce, Ability.KnockbackDuration, useSound: false);
            Burn burnEffect = new(GameData.player, enemy, damage: Ability.damage * 0.1f);
            enemy.Statuses.ApplyEffect(burnEffect);
            Destroy(gameObject);
            Ability.StartCooldown();
        }
    }
}
