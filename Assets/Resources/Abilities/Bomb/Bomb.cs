using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : InstantiatedAbilityMono
{
    [SerializeField] private BombExplosion _explosionPrefab;

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
            ability.StartCooldown();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;

        enemy.TakeDamage(gameObject, ability.damage);

        if (_explosionPrefab != null)
        {
            BombExplosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.Init(timer, ability);
        }

        Destroy(gameObject);
    }
}
