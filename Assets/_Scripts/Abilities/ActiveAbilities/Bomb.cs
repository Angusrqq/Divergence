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
    protected override void Start()
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
            return;
        }
        base.Start();
    }

    public override void EnemyCollision(Enemy enemy)
    {
        if (isExploded) return;

        enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage"));

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
                enemy.TakeDamage(GameData.player.gameObject, Ability.GetStat("Damage") * 2, GetType());
            }
        }
        yield return new WaitForSeconds(animatedEntity.AnimatorController.animationClips[1].length);
        
        Destroy(gameObject);
    }
}
