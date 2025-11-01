using UnityEngine;
using System.Linq;

// TODO: Evgeniy - Refactor this
/// <summary>
/// Base MonoBehaviour for runtime ability instances spawned by
/// <see cref="InstantiatedAbilityScriptable"/> during <c>Activate()</c>.
/// Handles movement, lifetime countdown, collision damage, and self-unregistration.
/// </summary>
/// <remarks>
/// Instances are created by <see cref="InstantiatedAbilityScriptable.Activate"/>, which calls
/// <see cref="Init(InstantiatedAbilityScriptable)"/> and adds the instance to
/// <see cref="InstantiatedAbilityScriptable.Instances"/>. This component moves every physics tick
/// using the player's current movement vector or facing as a fallback, and destroys itself when the
/// configured active time elapses. On destruction, it removes itself from the owning ability's
/// <see cref="InstantiatedAbilityScriptable.Instances"/> list.
/// </remarks>
[RequireComponent(typeof(Rigidbody2D))]
public class InstantiatedAbilityMono : MonoBehaviour
{
    [System.NonSerialized] private InstantiatedAbilityScriptable _ability;
    public InstantiatedAbilityScriptable Ability
    {
        get => _ability;
        protected set => _ability = value;
    }
    
    protected Rigidbody2D rb;
    protected Vector2 direction;
    protected float timer;

    /// <summary>
    /// Caches the <see cref="Rigidbody2D"/> and derives the initial movement direction
    /// from the player's movement vector or facing if idle.
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = GameData.player.MovementVector;

        if (direction == Vector2.zero)
        {
            direction = new Vector2(GameData.player.SpriteRenderer.flipX ? -1 : 1, 0);
        }
    }

    /// <summary>
    /// Initializes the instance with its owning ability and lifetime.
    /// Called by <see cref="InstantiatedAbilityScriptable.Activate"/> right after instantiation.
    /// </summary>
    /// <param name="ability">The ability data providing speed, damage, and ActiveTime.</param>
    public virtual void Init(InstantiatedAbilityScriptable ability)
    {
        Ability = ability;
        timer = ability.ActiveTime;
    }

    /// <summary>
    /// Physics tick: applies movement logic and counts down remaining active time.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        FixedUpdateLogic();
        CountDownActiveTimer(Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// Default straight-line movement at <see cref="Ability.speed"/> in the resolved <see cref="direction"/>.
    /// Override to provide custom per-ability trajectories.
    /// </summary>
    protected virtual void FixedUpdateLogic()
    {
        rb.MovePosition(Ability.speed * direction + rb.position);
    }

    /// <summary>
    /// Decrements the internal lifetime and destroys the GameObject when it expires.
    /// </summary>
    /// <param name="delta">The fixed time step to subtract from the remaining lifetime.</param>
    protected virtual void CountDownActiveTimer(float delta)
    {
        timer -= delta;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// On destruction, unregisters this instance from <see cref="InstantiatedAbilityScriptable.Instances"/>.
    /// </summary>
    protected virtual void OnDestroy()
    {
        Ability.Instances.Remove(this);
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
            enemy.TakeDamage(GameData.player.gameObject, Ability.damage, GetType(), Ability.KnockbackForce, Ability.KnockbackDuration);
        }
    }
    
    /// <summary>
    /// Returns the enemy closest to the player, or <c>null</c> if none exist.
    /// </summary>
    /// <returns>The nearest <see cref="Enemy"/> to the player, or <c>null</c>.</returns>
    public static Enemy FindClosest()
    {
        return EnemyManager.Enemies.OrderBy(enemy => Vector2.Distance(enemy.transform.position, GameData.player.transform.position)).FirstOrDefault(enemy => enemy != null);
    }
}
