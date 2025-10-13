using UnityEngine;

/// <summary>
/// <para>
/// Example use of an ability.
/// </para>
/// </summary>
public class TestAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    [System.NonSerialized] public ThrowObjectTest ability;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = GameData.player.movementVector;
        if (direction == Vector2.zero) direction = new Vector2(GameData.player.spriteRenderer.flipX ? -1 : 1, 0);
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
