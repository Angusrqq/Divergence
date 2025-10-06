using System;
using UnityEngine;

/// <summary>
/// <para>
/// <c>Magnet</c> is a script that makes the xp crystals magnet to the player.
/// </para>
/// It basically checks if the xp crystal is in the magnet's radius and if it is,
/// it starts the xp crystal's <c>MagnetToPlayerCoroutine</c> passing the serialized animation curve.
/// </summary>
public class Magnet : MonoBehaviour
{
    [NonSerialized] public CircleCollider2D magnetCollider;
    public AnimationCurve curve;

    void Start()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        UpdateRadius();
        Attributes.OnAttributeChanged += HandleAttributeChanged;
    }

    public void UpdateRadius()
    {
        magnetCollider.radius = Attributes.magnetRadius;
    }

    void OnDestroy()
    {
        Attributes.OnAttributeChanged -= HandleAttributeChanged;
    }

    /// <summary>
    /// <para>
    /// <c>HandleAttributeChanged</c> is a method for updating the magnet's radius if the attribute changed (for example, if the player upgrades the magnet radius).
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
