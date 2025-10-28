using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BombExplosion : InstantiatedAbilityMono
{
    public void Init(float timer, InstantiatedAbilityScriptable ability)
    {
        this.timer = timer;
        this.ability = ability;
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        CountDownActiveTimer(Time.fixedDeltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(gameObject, ability.damage * 2, knockbackForce: ability.KnockbackForce, knockbackDuration: ability.KnockbackDuration);
        }
    }
}
