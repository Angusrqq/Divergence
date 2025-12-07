using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class PrayerShockwave : MonoBehaviour
{
    private const float _MAX_COLLIDER_RADIUS = 0.5f;
    private const float _START_COLLIDER_RADIUS = 0f;
    
    [NonSerialized] public float Speed = 2f;
    [NonSerialized] public float Radius = 40f;
    [NonSerialized] public float Duration = 1f;

    private CircleCollider2D _collider;
    private Material _material;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _material = GetComponent<SpriteRenderer>().material;
        
        Physics2D.IgnoreCollision(_collider, GameData.player.GetComponent<Collider2D>(), true);
        StartCoroutine(LerpCoroutine());
        transform.localScale = new Vector3(Radius, Radius, 0f);
    }

    private IEnumerator LerpCoroutine()
    {
        float timePassed = 0f;
        while(timePassed < Duration)
        {
            timePassed += Time.deltaTime * Speed;
            float t = timePassed / Duration;

            _collider.radius = Mathf.Lerp(_START_COLLIDER_RADIUS, _MAX_COLLIDER_RADIUS, t);
            _material.SetFloat("_WaveDistanceFromCenter", _collider.radius);
            _material.SetFloat("_Size", Mathf.Lerp(0.05f, 0f, t));

            transform.position = GameData.player.transform.position;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
