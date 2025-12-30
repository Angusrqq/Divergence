using UnityEngine;
using System.Collections.Generic;
using System;

public class InstantiatedAbilityMono : BaseAbilityMono
{
    [NonSerialized] public Enemy Target;
    public AudioClip OnActivation;
    public AudioClip OnHit;
    public Action<InstantiatedAbilityMono> OnDeath;

    protected Rigidbody2D rb;
    protected AnimatedEntity animatedEntity;
    protected Vector2 direction;
    protected float timer;
    protected bool doesDamage = true;

    [NonSerialized] private InstantiatedAbilityHandler _ability;
    private bool _hit = false;

    public InstantiatedAbilityHandler Ability
    {
        get => _ability;
        protected set => _ability = value;
    }

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
            AudioManager.instance.PlaySound(Ability.AudioSource, UnityEngine.Random.Range(0.95f, 1.05f), OnActivation);
        }
    }

    public virtual void Init(InstantiatedAbilityHandler ability)
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

    protected virtual void OnDestroy()
    {
        OnDeath?.Invoke(this);
        Ability?.Instances.Remove(this);
        OnDeath = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (OnHit != null && !_hit)
            {
                AudioManager.instance.PlaySound(Ability.AudioSource, UnityEngine.Random.Range(0.95f, 1.05f), OnHit);
                _hit = true;
            }

            EnemyCollision(enemy);
            if (doesDamage)
            {
                GameData.player.AbilityHolder.TriggerOnEnemyHit(GetType(), enemy, Ability.GetStat("Damage"), this);
            }
        }

        OtherCollision(other);
        GameData.player.AbilityHolder.TriggerOnProjectileHit(GetType(), rb.position);
    }

    public virtual void EnemyCollision(Enemy enemy)
    {
        enemy.TakeDamage(
            GameData.player.gameObject,
            Ability.GetStat("Damage"),
            GetType(),
            Ability.GetStat("Knockback Force"),
            Ability.KnockbackDuration
        );
    }

    /// <summary>
    /// Called when another object enters a 2D collider trigger that is not an Enemy.
    /// </summary>
    protected virtual void OtherCollision(Collider2D other) { }

    public static Enemy FindClosest()
    {
        var enemies = EnemyManager.Enemies;
        if (enemies == null || enemies.Count == 0) return null;

        var playerPos = GameData.player.transform.position;
        Enemy closest = null;
        float minDist = float.MaxValue;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (enemy == null) continue;

            float dist = (enemy.transform.position - playerPos).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
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
        target = FindClosest();
        if (target == null)
        {
            target = FindClosest();
        }
    }
    
    public virtual void Upgrade(InstantiatedAbilityHandler ability) { }
}
