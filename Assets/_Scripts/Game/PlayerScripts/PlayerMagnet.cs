using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

/// <summary>
/// <para>
/// <c>Magnet</c> is a script that makes the xp crystals magnet to the player.
/// </para>
/// It basically checks if the xp crystal is in the magnet's radius and if it is,
/// it starts the xp crystal's <c>MagnetToPlayerCoroutine</c> passing the serialized animation curve.
/// </summary>
public class PlayerMagnet : MonoBehaviour
{
    [NonSerialized] public CircleCollider2D magnetCollider;
    public AnimationCurve curve;

    /// <summary>
    /// Called when the script instance is being initialized.
    /// <para>
    /// It gets the <c>CircleCollider2D</c> component, updates the radius of the magnet to the value of the <c>MagnetRadius</c> attribute and subscribes to the <c>OnAttributeChanged</c> event to handle the attribute changed.
    /// </para>
    /// </summary>
    void Start()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        UpdateRadius();
        Attributes.OnAttributeChanged += HandleAttributeChanged;
    }

    /// <summary>
    /// Updates the radius of the magnet to the value of the <c>MagnetRadius</c> attribute.
    /// </summary>
    public void UpdateRadius()
    {
        magnetCollider.radius = Attributes.MagnetRadius;
    }

    /// <summary>
    /// Called when the script instance is being destroyed.
    /// Unsubscribes from the <c>Attributes.OnAttributeChanged</c> event to avoid memory leaks.
    /// </summary>
    void OnDestroy()
    {
        Attributes.OnAttributeChanged -= HandleAttributeChanged;
    }

    /// <summary>
    /// <para>
    /// <c>HandleAttributeChanged</c> is a method for updating the magnet's radius if the attribute changed.
    /// </para>
    /// </summary>
    private void HandleAttributeChanged(AttributeId id, float value)
    {
        if (id != AttributeId.MagnetRadius) return;
        if (magnetCollider == null)
        {
            magnetCollider = GetComponent<CircleCollider2D>();
            if (magnetCollider == null) return;
        }
        magnetCollider.radius = value;
    }

    /// <summary>
    /// Called when the magnet's trigger area is entered by another object.
    /// </summary>
    /// <param name="collision">The other object that entered the trigger area.</param>
    /// <remarks>
    /// If the other object is an <c>XpCrystal</c> and it is not already being attracted to the player, it starts the XP crystal's <c>MagnetToPlayerCoroutine</c> passing the serialized animation curve.
    /// </remarks>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out XpCrystal xpCrystal))
        {
            if (xpCrystal != null && !xpCrystal.IsFired)
            {
                xpCrystal.StartCoroutine(xpCrystal.MagnetToPlayerCoroutine(curve));
            }
        }
    }
}
