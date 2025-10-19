using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : MonoBehaviour
{
    private Vector2 lastPosition;
    private SpriteRenderer spriteRenderer;
    public bool applyCollisionToSelf = true;
    public bool applyCollisionToOther = true;
    private static readonly List<GridObject> nearbyBuffer = new();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;

        SpatialGridSystem.Instance.Grid.Register(this, transform.position);
    }

    void Update()
    {
        Vector2 pos = transform.position;
        if ((pos - lastPosition).sqrMagnitude > 1f) // moved enough
        {
            SpatialGridSystem.Instance.Grid.UpdateObjectPosition(this, pos);
            lastPosition = pos;
        }
    }
    
    void FixedUpdate()
    {
        var grid = SpatialGridSystem.Instance.Grid;
        grid.GetNearby(Position, nearbyBuffer);

        foreach (var other in nearbyBuffer)
        {
            if (other == this) continue;

            Vector2 delta = other.Position - Position;
            float dist = delta.magnitude;
            float minDist = Radius + other.Radius;

            if (dist < minDist && dist > 0.0001f)
            {
                float overlap = minDist - dist;
                Vector2 direction = delta / dist;

                // Push both objects apart slightly
                Vector2 correction = direction * (overlap / 2f);
                if (applyCollisionToSelf && other.applyCollisionToOther)
                {
                    transform.position -= (Vector3)correction;
                }
                if (applyCollisionToOther && other.applyCollisionToSelf)
                {
                    other.transform.position += (Vector3)correction;
                }

                Debug.DrawLine(Position, other.Position, Color.red);
            }
        }
    }


    void OnDestroy()
    {
        // Clean up on removal
        if (SpatialGridSystem.Instance != null)
            SpatialGridSystem.Instance.Grid.Unregister(this);
    }

    public float Radius => Mathf.Max(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);

    public Vector2 Position => transform.position;
}
