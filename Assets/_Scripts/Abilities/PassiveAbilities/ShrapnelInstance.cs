using System.Collections;
using UnityEngine;

public class ShrapnelInstance : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private AnimatedEntity _animatedEntity;
    private float _damage;
    private Vector2 _direction;
    private float _speed;
    private Rigidbody2D _rb;
    private bool _hit = false;
    private Enemy _hitEnemy = null;
    private float _maxActiveTime = 2f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animatedEntity = GetComponent<AnimatedEntity>();

        StartCoroutine(Timer());
    }

    public void Init(float damage, float radius, Vector2 direction, float speed)
    {
        _damage = damage;
        transform.localScale = new Vector3(radius, radius, 0f);
        _direction = direction;
        _speed = speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) && !_hit)
        {
            enemy.TakeDamage(GameData.player.gameObject, _damage, GetType(), 10f, 0.2f, useSound: true);
            _hit = true;
            _hitEnemy = enemy;
            _animatedEntity.ChangeAnimation("Shrapnel_hit");
            StopAllCoroutines();
            StartCoroutine(HitFlash(2f));
        }
    }

    void FixedUpdate()
    {
        if (!_hit)
        {
            _rb.MovePosition(_direction * _speed + _rb.position);
        }
        if (_hitEnemy != null)
        {
            _rb.MovePosition(_hitEnemy.transform.position);
        }
    }

    private IEnumerator HitFlash(float duration)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            _spriteRenderer.color = Color.Lerp(Color.white, Color.red, timePassed / duration);
            timePassed += Time.fixedDeltaTime;

            yield return null;
        }
    }
    
    public void LastFrame()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void DamageFrame()
    {
        if (_hitEnemy != null)
        {
            _hitEnemy.TakeDamage(
                source: GameData.player.gameObject,
                amount: _damage / 4,
                type: GetType(),
                knockbackForce: 3f,
                knockbackDuration: 0.2f,
                useSound: true
            );
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_maxActiveTime);
        Destroy(gameObject);
    }
}
