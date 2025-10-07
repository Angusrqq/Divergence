using System.Collections;
using UnityEngine;

public class XpCrystal : MonoBehaviour
{
    private int xpValue;
    public bool IsFired { get; private set; } = false; // Fired from job
    public float speed = 2f;

    public void SetXpValue(int xpValue)
    {
        if (xpValue > 0)
        {
            this.xpValue = xpValue;
        }
        else
        {
            Debug.LogError("xpCrystal xpValue must be greater than 0");
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
                time += Time.fixedDeltaTime * speed;
                transform.position = Vector3.Lerp(transform.position, GameData.player.transform.position, curve.Evaluate(time));
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExp(gameObject, xpValue);
            Destroy(gameObject);
        }
    }
}
