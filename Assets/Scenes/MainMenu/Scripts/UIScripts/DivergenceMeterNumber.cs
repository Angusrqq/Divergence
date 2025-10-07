using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteResolver))]
public class DivergenceMeterNumber : MonoBehaviour
{
    [NonSerialized] public SpriteResolver resolver;
    private const string category = "DivergenceMeterSheet";
    public bool Rolled = false;

    void Awake()
    {
        resolver = GetComponent<SpriteResolver>();
    }

    public void CustomUpdate()
    {
        int rand = UnityEngine.Random.Range(0, 10);
        resolver.SetCategoryAndLabel(category, rand.ToString());
    }

    // TODO: DO FUCKING DOCUMENTATION FOR THIS SHIT EGOR
    public IEnumerator RollCoroutine(float time, float delay = 0f, bool blink = false, string label = "0")
    {
        while (time > 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);

            resolver.SetCategoryAndLabel(category, UnityEngine.Random.Range(0, 10).ToString());
            time -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        resolver.SetCategoryAndLabel(category, label);
        Rolled = true;
    }
}
