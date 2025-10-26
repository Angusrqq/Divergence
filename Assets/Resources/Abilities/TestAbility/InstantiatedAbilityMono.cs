using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// <para>
/// Example use of an ability.
/// </para>
/// </summary>
public class InstantiatedAbilityMono : MonoBehaviour
{
    [System.NonSerialized] public InstantiatedAbilityScriptable ability;
    
    protected Rigidbody2D rb;
    protected Vector2 direction;
    protected float timer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = GameData.player.MovementVector;

        if (direction == Vector2.zero)
        {
            direction = new Vector2(GameData.player.SpriteRenderer.flipX ? -1 : 1, 0);
        }
    }

    public virtual void Init(InstantiatedAbilityScriptable ability)
    {
        this.ability = ability;
        timer = ability.ActiveTime;
    }

    protected virtual void FixedUpdate()
    {
        FixedUpdateLogic();
        CountDownActiveTimer(Time.fixedDeltaTime);
    }
    
    protected virtual void FixedUpdateLogic()
    {
        rb.MovePosition(ability.speed * direction + rb.position);
    }

    protected virtual void CountDownActiveTimer(float delta)
    {
        timer -= delta;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called when another object enters a 2D collider trigger and checks if the other object has an Enemy component to apply damage, knockback force, and knockback duration to the enemy.
    /// </summary>
    /// <param name="other">The Collider2D of the other object</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object has an Enemy component
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            // Apply damage, knockback force, and knockback duration to the enemy
            enemy.TakeDamage(gameObject, ability.damage, knockbackForce: ability.KnockbackForce, knockbackDuration: ability.KnockbackDuration);
        }
    }
}
