using System.Collections;
using UnityEngine;

public class ShrapnelInstance : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float _damage;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(float damage, float radius)
    {
        _damage = damage;
        transform.localScale = new Vector3(radius, radius, 0f);
    }

    public void AnimEvent()
    {
        Vector3 size = _spriteRenderer.bounds.extents * 2f;
        float radius = Mathf.Max(size.x, size.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GameData.player.transform.position, radius);
        bool hit = false;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(GameData.player.gameObject, _damage, GetType(), 10f, 0.2f);
                    hit = true;
                }
                
            }
        }
        if (hit)
        {
            StopAllCoroutines();
            StartCoroutine(HitFlash(0.01f));
        }
    }

    void FixedUpdate()
    {
        transform.position = GameData.player.transform.position;
    }

    private IEnumerator HitFlash(float duration)
    {
        float timePassed = 0f;
        while (timePassed < duration)
        {
            _spriteRenderer.color = Color.Lerp(Color.red, Color.white, timePassed / duration);
            yield return null;
        }
    }
    public void LastFrame()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
