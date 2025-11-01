using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an XP (Experience Points) crystal in the game that can be collected by the player.
/// The crystal moves towards the player when activated and awards XP when collected.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class XpCrystal : MonoBehaviour
{
    private const float SPEED = 1f;
    
    private int _xpValue;
    private SpriteRenderer _spriteRenderer;

    public bool IsFired { get; private set; }

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Sets the XP (experience points) value for the XP crystal.
    /// </summary>
    /// <param name="value">The XP value to set. Must be greater than 0.</param>
    /// <remarks>
    /// If the provided value is less than or equal to 0, an error will be logged and the game object will be destroyed.
    /// </remarks>
    public void SetXpValue(int value)
    {
        if (value > 0)
        {
            _xpValue = value;
        }
        else
        {
            Debug.LogError("XpCrystal xpValue must be greater than 0");
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

    /// <summary>
    /// Called when the XP crystal enters the trigger area of another object.
    /// </summary>
    /// <param name="other">The other object that entered the trigger area.</param>
    /// <remarks>
    /// If the other object is a <c>Player</c>, it adds the XP value of the XP crystal to the player's XP and then destroys the XP crystal.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExp(gameObject, _xpValue);
            Destroy(gameObject);
        }
    }
}
