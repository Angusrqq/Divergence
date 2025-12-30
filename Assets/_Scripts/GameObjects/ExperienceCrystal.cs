using System.Collections;
using UnityEngine;

public class ExperienceCrystal : MonoBehaviour
{
    private const byte SPEED = 1;

    private float _experience = 1f;
    [SerializeField] private float _lifetime = 180f;

    public bool IsFired { get; private set; }

    public static ExperienceCrystal Create(ExperienceCrystal prefab, Vector3 position, Transform parent, float experience = 1f)
    {
        ExperienceCrystal crystal = Instantiate(prefab, position, Quaternion.identity, parent);
        crystal._experience = experience;

        return crystal;
    }

    private void OnEnable()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExperience(gameObject, _experience);
            Destroy(gameObject);
        }
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(_lifetime);

        if (this != null && gameObject != null)
        {
            Destroy(gameObject);
        }
    }

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

    public IEnumerator PullToPlayerSmoothlyCoroutine(AnimationCurve curve)
    {
        IsFired = true;

        float t = 0f;

        while (gameObject != null)
        {
            if (GameData.player != null)
            {
                t += Time.fixedDeltaTime;

                float speed = curve.Evaluate(t) * 15f;

                Vector3 target = GameData.player.transform.position;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    speed * Time.fixedDeltaTime + 1
                );
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
