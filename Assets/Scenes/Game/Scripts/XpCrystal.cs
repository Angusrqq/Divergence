using System.Collections;
using UnityEngine;

public class XpCrystal : MonoBehaviour
{
    private int xpValue;
    public bool IsFired { get; private set; } = false;
    public float speed = 2f;

    public void SetxpValue(int xpValue)
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

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExp(gameObject, xpValue);
            Destroy(gameObject);
        }
    }
}
