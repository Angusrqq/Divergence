using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DivergenceMeter : MonoBehaviour
{
    private const string category = "DivergenceMeterSheet";
    private List<DivergenceMeterNumber> _numbers = new();
    [SerializeField] private AnimationCurve GlowCurve;
    public int Seed;
    [SerializeField] private Material DM_material;
    private Color defaultMaterialColor;
    private static bool CoroutineRunning = false;
    public static bool animationEnded = false;
    public enum AnimationVariant
    {
        Full,
        Fast
    }

    void Awake()
    {
        defaultMaterialColor = DM_material.GetColor("_Color");
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("dot")) continue;
            _numbers.Add(child.GetComponent<DivergenceMeterNumber>());
        }
        Seed = PlayerPrefs.GetInt("Seed", Random.Range(0, 1999999));
    }

    //TODO: turn numbers into parameters
    public IEnumerator PlayAnimation(float minRollTime = 1.5f, float maxRollTime = 3.5f, AnimationVariant variant = AnimationVariant.Full)
    {
        if (variant == AnimationVariant.Full)
        {
            foreach (DivergenceMeterNumber num in _numbers)
            {
                StartCoroutine(num.RollCoroutine(Random.Range(minRollTime, maxRollTime), label: GetDigitFromNumber(Seed, _numbers.IndexOf(num), _numbers.Count).ToString()));
            }
            while (!_numbers.All(x => x.Rolled))
            {
                yield return new WaitForEndOfFrame();
            }
            Coroutine Blink = StartCoroutine(GlowFade(DM_material, defaultMaterialColor, defaultMaterialColor * 10f, GlowCurve, 0.3f));//<< these numbers
            while (CoroutineRunning)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        if (variant == AnimationVariant.Fast)
        {
            foreach (DivergenceMeterNumber num in _numbers)
            {
                StartCoroutine(num.RollCoroutine(minRollTime, label: GetDigitFromNumber(Seed, _numbers.IndexOf(num), _numbers.Count).ToString()));
            }
            yield return new WaitForSeconds(minRollTime);
            Coroutine Blink = StartCoroutine(GlowFade(DM_material, defaultMaterialColor, defaultMaterialColor * 10f, GlowCurve, 0.3f));//<<
            while (CoroutineRunning)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        animationEnded = true;
        GameData.SetSeed(Seed);// should not be here
    }

    public static IEnumerator GlowFade(Material material, Color StartMatColor, Color TargetMatColor, AnimationCurve curve, float time)
    {
        CoroutineRunning = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            material.SetColor("_Color", Color.Lerp(StartMatColor, TargetMatColor, curve.Evaluate(time)));
            yield return new WaitForEndOfFrame();
        }
        CoroutineRunning = false;
    }

    public static int GetDigitFromNumber(int seed, int idx, int length = 0)
    {
        if (idx > seed.ToString().Length - 1 || length - idx > seed.ToString().Length) return 0;
        idx = length == 0 ? idx : length - idx - 1;
        for (int i = 0; i < idx; i++)
        {
            seed /= 10;
        }
        return seed % 10;
    }
}
