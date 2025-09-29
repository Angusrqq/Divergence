using UnityEngine;

public class TestAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    [System.NonSerialized] public ThrowObjectTest ability;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(ability.speed * direction + rb.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(this.gameObject, ability.damage, ability.knockbackForce, ability.knockbackDuration);
        }
    }
}
