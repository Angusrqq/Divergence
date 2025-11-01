using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteResolver))]
public class DivergenceMeterNumber : MonoBehaviour
{
    private SpriteResolver _resolver;
    public bool Rolled { get; private set; } = false;

    private const string CATEGORY = "DivergenceMeterSheet";

    void Awake()
    {
        _resolver = GetComponent<SpriteResolver>();
    }

    public void CustomUpdate()
    {
        int rand = Random.Range(0, 10);
        _resolver.SetCategoryAndLabel(CATEGORY, rand.ToString());
    }

    /// <summary>
    /// <para>
    /// Coroutine that rolls the digit on the divergence meter and sets the <c>Sprite</c> of the <c>SpriteResolver</c> to the passed <paramref name="label"></paramref>.
    /// </para>
    /// </summary>
    /// <param name="time">Duration of the animation in seconds</param>
    /// <param name="delay">Delay between each digit switch</param>
    /// <param name="label">Label of the <c>Sprite</c> that will be set after the animation is ended</param>
    /// <returns></returns>
    public IEnumerator RollCoroutine(float time, float delay = 0f, string label = "0")
    {
        while (time > 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);

            _resolver.SetCategoryAndLabel(CATEGORY, Random.Range(0, 10).ToString());
            time -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _resolver.SetCategoryAndLabel(CATEGORY, label);
        Rolled = true;
    }
}
