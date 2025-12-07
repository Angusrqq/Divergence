using System;
using System.Collections;
using UnityEngine;

public class DeathMarkInstance : MonoBehaviour
{
    public Action OnDeath;
    
    private SpriteRenderer _spriteRenderer;
    private AnimatedEntity _animatedEntity;
    private Enemy _target;
    private float _damage;
    private float _explosionRadius;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animatedEntity = GetComponent<AnimatedEntity>();
    }

    private void OnEnemyDeath(UnityEngine.Object sender)
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        _spriteRenderer.sortingOrder = 0;
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        transform.localScale = new Vector3(_explosionRadius, _explosionRadius, 0f);
        _animatedEntity.ChangeAnimation("MarkExplosion");
        Vector3 size = _spriteRenderer.bounds.extents;
        float radius = Mathf.Max(size.x, size.y);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (enemy == null) continue;

                enemy.TakeDamage(
                    source: gameObject,
                    amount: _damage,
                    type: GetType()
                );
            }
        }
        yield return new WaitForSeconds(_animatedEntity.AnimatorController.animationClips[1].length);

        Destroy(gameObject);
    }

    public void Init(Enemy target, float damage, float explosionRadius)
    {
        _damage = damage;
        _target = target;
        _explosionRadius = explosionRadius;
    }

    private void Start()
    {
        _target.GetComponent<DamageableEntity>().OnDeath += OnEnemyDeath;
        StartCoroutine(Idle());
    }

    private IEnumerator Idle(float speed = 4f)
    {
        float timer = 0f;
        float minDeg = -45f;
        float maxDeg = 45f;

        while (true)
        {
            if (_target == null)
            {
                OnEnemyDeath(null);
                yield break;
            }

            float t = (Mathf.Sin(timer * speed) + 1) / 2f;
            Vector3 offsetPos = _target.transform.position + new Vector3(0f, 0.5f, 0f);
            transform.SetPositionAndRotation(offsetPos, Quaternion.Euler(new Vector3(0f, 0f, Mathf.Lerp(minDeg, maxDeg, t))));
            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        OnDeath = null;
    }
}
