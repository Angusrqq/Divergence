using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : InstantiatedAbilityMono
{
    private bool isExploded = false;

    /// <summary>
    /// Caches required components and initializes base state.
    /// </summary>
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent(out animatedEntity);
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

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploded) return;
        if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;

        enemy.TakeDamage(gameObject, Ability.damage);

        isExploded = true;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        direction = Vector2.zero;
        gameObject.transform.localScale *= 5f;
        animatedEntity.ChangeAnimation("BombExplosionClip");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(gameObject, Ability.damage * 2);
            }
        }
        yield return new WaitForSeconds(animatedEntity.AnimatorController.animationClips[1].length); // wonky shit picking animation by index
        Destroy(gameObject);
    }
}
