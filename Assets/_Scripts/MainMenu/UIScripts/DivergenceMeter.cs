using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DivergenceMeter : MonoBehaviour
{
    private const string CATEGORY = "DivergenceMeterSheet";

    [SerializeField] private AnimationCurve _glowCurve;
    [SerializeField] private Material _divergenceMeterMaterial;

    public Coroutine NestedCoroutine;
    public static bool AnimationEnded = false;

    private int _seed;
    private readonly List<DivergenceMeterNumber> _numbers = new();
    private Color _defaultMaterialColor;

    public enum AnimationVariant
    {
        Full,
        Fast
    }

    void Awake()
    {
        AnimationEnded = false;
        _defaultMaterialColor = _divergenceMeterMaterial.GetColor("_Color");

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("dot")) continue;
            _numbers.Add(child.GetComponent<DivergenceMeterNumber>());
        }

        Seed = PlayerPrefs.GetInt("Seed", Random.Range(0, 1999999));
    }

    void OnDestroy()
    {
        _divergenceMeterMaterial.SetColor("_Color", _defaultMaterialColor);
    }

    public IEnumerator PlayAnimation(float minRollTime = 1.5f, float maxRollTime = 3.5f, AnimationVariant variant = AnimationVariant.Full)
    {
        _divergenceMeterMaterial.SetColor("_Color", _defaultMaterialColor);
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
        }

        else if (variant == AnimationVariant.Fast)
        {
            foreach (DivergenceMeterNumber num in _numbers)
            {
                StartCoroutine(num.RollCoroutine(minRollTime, label: GetDigitFromNumber(Seed, _numbers.IndexOf(num), _numbers.Count).ToString()));
            }

            yield return new WaitForSeconds(minRollTime);
        }

        yield return StartCoroutine(GlowFade(_divergenceMeterMaterial, _defaultMaterialColor, _defaultMaterialColor * 20f, _glowCurve, 0.3f));//<< these numbers

        AnimationEnded = true;
        GameData.SetSeed(Seed);
    }

    public IEnumerator IdleAnimation(float time = 1.5f, AnimationVariant variant = AnimationVariant.Full)
    {
        while (true)
        {
            yield return GlowFade(_divergenceMeterMaterial, _defaultMaterialColor * 5f, Color.black, time);

            foreach (DivergenceMeterNumber num in _numbers)
            {
                num.CustomUpdate();
            }

            yield return GlowFade(_divergenceMeterMaterial, _divergenceMeterMaterial.GetColor("_Color"), _defaultMaterialColor * 5f, time);
        }
    }

    /// <summary>
    /// Fancy lerp for the material colors
    /// </summary>
    /// <param name="material">the material to change its color</param>
    /// <param name="startMatColor">Color <c>a</c> for linear interpolation</param>
    /// <param name="targetMatColor">Color <c>b</c> for linear interpolation</param>
    /// <param name="curve">Curve (from 0 to 1 on time) that will be evaluated by <paramref name="time"></paramref></param>
    /// <param name="time">How fast the color should change (less time -> more speed)</param>
    public static IEnumerator GlowFade(Material material, Color startMatColor, Color targetMatColor, AnimationCurve curve, float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            material.SetColor("_Color", Color.Lerp(startMatColor, targetMatColor, curve.Evaluate(time)));

            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator GlowFade(Material material, Color startMatColor, Color targetMatColor, float time)
    {
        float timePassed = time;

        while (timePassed > 0)
        {
            timePassed -= Time.deltaTime;
            material.SetColor("_Color", Color.Lerp(startMatColor, targetMatColor, 1 - (timePassed / time)));

            yield return new WaitForEndOfFrame();
        }
    }

    public static int GetDigitFromNumber(int number, int index, int length = 0)
    {
        if (index > number.ToString().Length - 1 || length - index > number.ToString().Length) return 0;

        index = length == 0 ? index : length - index - 1;

        for (int i = 0; i < index; i++)
        {
            number /= 10;
        }

        return number % 10;
    }

    public int Seed
    {
        get => _seed;
        private set => _seed = value;
    }
}
