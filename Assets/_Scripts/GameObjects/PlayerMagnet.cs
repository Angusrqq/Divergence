using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMagnet : MonoBehaviour
{
    [NonSerialized] public CircleCollider2D magnetCollider;
    public AnimationCurve curve;

    void Start()
    {
        magnetCollider = GetComponent<CircleCollider2D>();

        UpdateRadius();
        GameData.InGameAttributes.OnAttributeChanged += HandleAttributeChanged;
    }

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

    void OnDestroy()
    {
        GameData.InGameAttributes.OnAttributeChanged -= HandleAttributeChanged;
    }

    public void UpdateRadius()
    {
        magnetCollider.radius = GameData.InGameAttributes.MagnetRadius;
    }

    private void HandleAttributeChanged(AttributeId id, Stat value)
    {
        if (id != AttributeId.MagnetRadius) return;

        magnetCollider.radius = value;
    }
}
