using System;
using UnityEngine;

/// <summary>
/// <para>
/// <c>Magnet</c> is a script that makes the xp crystals magnet to the player.
/// </para>
/// It basically checks if the xp crystal is in the magnet's radius and if it is,
/// it starts the xp crystal's <c>MagnetToPlayerCoroutine</c> passing the serialized animation curve.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
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
        GameData.InGameAttributes.OnAttributeChanged += HandleAttributeChanged;
    }

    /// <summary>
    /// Called when the magnet's trigger area is entered by another object.
    /// If the other object is an <c>ExperienceCrystal</c> and it is not fired, it starts the experience crystal's <c>MagnetToPlayerCoroutine</c> passing the serialized animation curve.
    /// </summary>
    /// <param name="collision">The collider of the object that entered the trigger area.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ExperienceCrystal experienceCrystal))
        {
            if (experienceCrystal != null && !experienceCrystal.IsFired)
            {
                experienceCrystal.StartCoroutine(experienceCrystal.MagnetToPlayerCoroutine(curve));
            }
        }
    }

    /// <summary>
    /// Called when the script instance is being destroyed.
    /// Unsubscribes from the <c>Attributes.OnAttributeChanged</c> event to avoid memory leaks.
    /// </summary>
    void OnDestroy()
    {
        GameData.InGameAttributes.OnAttributeChanged -= HandleAttributeChanged;
    }

    /// <summary>
    /// Updates the radius of the magnet to the value of the <c>MagnetRadius</c> attribute.
    /// </summary>
    public void UpdateRadius()
    {
        magnetCollider.radius = GameData.InGameAttributes.MagnetRadius;
    }

    /// <summary>
    /// <para>
    /// <c>HandleAttributeChanged</c> is a method for updating the magnet's radius if the attribute changed.
    /// </para>
    /// </summary>
    private void HandleAttributeChanged(AttributeId id, Stat value)
    {
        if (id != AttributeId.MagnetRadius) return;

        if (magnetCollider == null)
        {
            magnetCollider = GetComponent<CircleCollider2D>();
            if (magnetCollider == null) return;
        }

        magnetCollider.radius = value;
    }
}
