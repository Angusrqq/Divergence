using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ObjectMagnet : MonoBehaviour
{
    public AnimationCurve curve;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var enemyManager = EnemyManager.Instance.transform;
        if (enemyManager == null) return;

        for (int i = 0; i < enemyManager.childCount; i++)
        {
            if (enemyManager.GetChild(i).TryGetComponent(out ExperienceCrystal crystal))
            {
                if (!crystal.IsFired)
                {
                    crystal.StartCoroutine(crystal.MagnetToPlayerCoroutine(curve));
                }
            }
        }

        Destroy(gameObject);
    }
}
