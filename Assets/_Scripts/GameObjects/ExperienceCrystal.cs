using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ExperienceCrystal : MonoBehaviour
{
    private const byte SPEED = 1;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _lifetime = 180f;

    public bool IsFired { get; private set; }

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Called when the <c>ExperienceCrystal</c> is enabled.
    /// Starts a coroutine that will destroy the experience crystal after a certain amount of time.
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    /// <summary>
    /// Called when the <c>ExperienceCrystal</c> enters the trigger area of another object.
    /// </summary>
    /// <param name="other">The other object that entered the trigger area.</param>
    /// <remarks>
    /// If the other object is a <c>Player</c>, it adds the 1 experience point to the player and then destroys the experience crystal.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExperience(gameObject, 1);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Coroutine that counts down the lifetime of the experience crystal and destroys it when the lifetime expires.
    /// </summary>
    /// <remarks>
    /// The coroutine waits for <c>_lifetime</c> seconds and then destroys the GameObject if it still exists.
    /// </remarks>
    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(_lifetime);

        if (this != null && gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Coroutine that moves the game object towards the player position using an animation curve.
    /// </summary>
    /// <param name="curve">Animation curve used to control the interpolation speed over time.</param>
    /// <returns>IEnumerator used for coroutine execution.</returns>
    public IEnumerator MagnetToPlayerCoroutine(AnimationCurve curve)
    {
        IsFired = true;
        float time = 0f;

        while (gameObject != null && time <= 1f)
        {
            if (GameData.player != null)
            {
                time += Time.fixedDeltaTime * SPEED;
                transform.position = Vector3.LerpUnclamped(transform.position, GameData.player.transform.position, curve.Evaluate(time));
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
