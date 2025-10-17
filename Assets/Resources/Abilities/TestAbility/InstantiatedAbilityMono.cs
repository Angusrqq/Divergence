using UnityEngine;

/// <summary>
/// <para>
/// Example use of an ability.
/// </para>
/// </summary>
public class InstantiatedAbilityMono : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Vector2 direction;
    [System.NonSerialized] public InstantiatedAbilityScriptable ability;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = GameData.player.movementVector;
        if (direction == Vector2.zero) direction = new Vector2(GameData.player.spriteRenderer.flipX ? -1 : 1, 0);
    }

    protected virtual void FixedUpdate()
    {
        rb.MovePosition(ability.speed * direction + rb.position);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(this.gameObject, ability.damage, ability.knockbackForce, ability.knockbackDuration);
        }
    }
}
