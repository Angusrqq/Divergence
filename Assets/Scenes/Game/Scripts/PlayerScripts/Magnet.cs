using System;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [NonSerialized] public CircleCollider2D magnetCollider;
    public AnimationCurve curve;

    void Start()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        UpdateRadius();
    }

    public void UpdateRadius()
    {
        magnetCollider.radius = Attributes.magnetRadius;
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
