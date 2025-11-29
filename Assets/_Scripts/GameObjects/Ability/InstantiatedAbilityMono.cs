using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// Base MonoBehaviour for runtime ability instances spawned by
/// <see cref="InstantiatedAbilityScriptable"/> during <c>Activate()</c>.
/// Handles movement, lifetime countdown, collision damage, and self-unregistration.
/// </summary>
/// <remarks>
/// Instances are created by <see cref="InstantiatedAbilityScriptable.Activate"/>, which calls
/// <see cref="Init"/> and adds the instance to
/// <see cref="InstantiatedAbilityScriptable.Instances"/>. This component moves every physics tick
/// using the player's current movement vector or facing as a fallback, and destroys itself when the
/// configured active time elapses. On destruction, it removes itself from the owning ability's
/// <see cref="InstantiatedAbilityScriptable.Instances"/> list.
/// </remarks>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class InstantiatedAbilityMono : MonoBehaviour
{
    [NonSerialized] private InstantiatedAbilityHandler _ability;
    private bool _hit = false;

    protected Rigidbody2D rb;
    protected AnimatedEntity animatedEntity;
    protected Vector2 direction;
    protected float timer;
    protected bool doesDamage = true;

    [NonSerialized] public Enemy Target;

    public InstantiatedAbilityHandler Ability { get => _ability; protected set => _ability = value; }
    public Stat Damage;
    public Stat KnockbackForce;
    public AudioClip OnActivation;
    public AudioClip OnHit;
    public Action<InstantiatedAbilityMono> OnDeath;

    /// <summary>
    /// Caches the <see cref="Rigidbody2D"/> and derives the initial movement direction
    /// from the player's movement vector or facing if idle.
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent(out animatedEntity);
        direction = GameData.player.MovementVector;

        if (direction == Vector2.zero)
        {
            direction = new Vector2(GameData.player.SpriteRenderer.flipX ? -1 : 1, 0);
        }
    }

    protected virtual void Start()
    {
        if (OnActivation != null)
        {
            //AudioManager.instance.PlaySFXPitched(OnActivation, UnityEngine.Random.Range(0.95f, 1.05f));
            AudioManager.instance.PlaySound(Ability.AudioSource, UnityEngine.Random.Range(0.95f, 1.05f), OnActivation);
        }
    }

    /// <summary>
    /// Initializes the instance with its owning ability and lifetime.
    /// Called by <see cref="InstantiatedAbilityHandler.Activate"/> right after instantiation.
    /// </summary>
    /// <param name="ability">The ability data providing speed, damage, and ActiveTime.</param>
    public virtual void Init(InstantiatedAbilityHandler ability)
    {
        Ability = ability;
        Damage = (float)ability.damage;
        KnockbackForce = (float)ability.KnockbackForce;
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
        rb.MovePosition(Ability.Speed * direction + rb.position);
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
        OnDeath?.Invoke(this);
        Ability.Instances.Remove(this);
        OnDeath = null;
    }

    /// <summary>
    /// Called when another object enters a 2D collider trigger and checks if the other object has an Enemy component to apply damage, knockback force, and knockback duration to the enemy.
    /// </summary>
    /// <param name="other">The Collider2D of the other object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object has an Enemy component
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            if(OnHit != null && !_hit)
            {
                //AudioManager.instance.PlaySFXPitched(OnHit, UnityEngine.Random.Range(-3f, 3f));
                AudioManager.instance.PlaySound(Ability.AudioSource, UnityEngine.Random.Range(0.95f, 1.05f), OnHit);
                _hit = true;
            }
            EnemyCollision(enemy);
            if (doesDamage)
            {
                GameData.player.AbilityHolder.TriggerOnEnemyHit(GetType(), enemy, Damage, this);
            }
        }
        OtherCollision(other);
        GameData.player.AbilityHolder.TriggerOnProjectileHit(GetType(), rb.position);
    }

    /// <summary>
    /// Called when another object enters a 2D collider trigger that is an Enemy.
    /// </summary>
    /// <param name="enemy"></param>
    public virtual void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(GameData.player.gameObject, Damage, GetType(), KnockbackForce, Ability.KnockbackDuration);
    }
    /// <summary>
    /// Called when another object enters a 2D collider trigger that is not an Enemy.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OtherCollision(Collider2D other)
    {
        // override
    }

    /// <summary>
    /// Returns the enemy closest to the player, or <c>null</c> if none exist.
    /// </summary>
    /// <returns>The nearest <see cref="Enemy"/> to the player, or <c>null</c>.</returns>
    public static Enemy FindClosest()
    {
        return EnemyManager.Enemies.OrderBy(enemy => Vector2.Distance(enemy.transform.position, GameData.player.transform.position)).FirstOrDefault(enemy => enemy != null);
    }

    /// <summary>
    /// Returns the closest enemy from a provided list of enemies, or <c>null</c> if none exist.
    /// </summary>
    /// <param name="enemies">List of enemies to search</param>
    /// <returns></returns>
    public static Enemy FindClosest(List<Enemy> enemies)
    {
        return enemies.OrderBy(enemy => Vector2.Distance(enemy.transform.position, GameData.player.transform.position)).FirstOrDefault(enemy => enemy != null);
    }

    public static void GetTargetForProjectile(InstantiatedAbilityHandler ability,out Enemy target)
    {
        List<Enemy> enemies = new(EnemyManager.Enemies);
        foreach (InstantiatedAbilityMono instance in ability.Instances)
        {
            if (instance.Target != null)
            {
                enemies.Remove(instance.Target);
            }
        }
        target = FindClosest(enemies);
        if (target == null)
        {
            target = FindClosest();
        }
    }
    
    public virtual void Upgrade(InstantiatedAbilityHandler ability)
    {
        
    }
}
