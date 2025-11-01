using UnityEngine;

/// <summary>
/// Represents a thrown bomb ability instance that seeks toward the nearest enemy
/// and explodes on contact. When the bomb collides with an <see cref="Enemy"/>,
/// it deals damage, optionally spawns an explosion effect, and then destroys itself.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : InstantiatedAbilityMono
{
    [SerializeField] private BombExplosion _explosionPrefab;

    /// <summary>
    /// Caches required components and initializes base state.
    /// </summary>
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// Acquires the closest enemy and sets the initial movement direction.
    /// If no enemy is found, the bomb is destroyed and the ability cooldown starts.
    /// </summary>
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

    /// <summary>
    /// Handles impact with colliders. If an <see cref="Enemy"/> is hit, applies damage,
    /// spawns the explosion effect (if assigned), and destroys the bomb.
    /// </summary>
    /// <param name="other">The collider that the bomb has entered.</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;

        enemy.TakeDamage(gameObject, Ability.damage);

        if (_explosionPrefab != null)
        {
            BombExplosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.Init(timer, Ability);
        }

        Destroy(gameObject);
    }
}
