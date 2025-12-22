using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls the Divergence Meter animation sequence and seed display on the main menu.
/// </summary>
/// <remarks>
/// Collects child <see cref="DivergenceMeterNumber"/> components, rolls digits to reveal the saved seed,
/// plays a glow effect on the associated material, and writes the seed to <see cref="GameData"/> when done.
/// </remarks>
public class DivergenceMeter : MonoBehaviour
{
    [SerializeField] private AnimationCurve _glowCurve;
    [SerializeField] private Material _divergenceMeterMaterial;

    public Coroutine NestedCoroutine;
    public static bool AnimationEnded = false;

    private int _seed;
    private readonly List<DivergenceMeterNumber> _numbers = new();
    private Color _defaultMaterialColor;

    public int Seed
    {
        get => _seed;
        private set => _seed = value;
    }

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

    /// <summary>
    /// Plays the roll animation for all digits and a glow pulse, then commits the seed to <see cref="GameData"/>.
    /// </summary>
    /// <param name="minRollTime">Minimum time per digit roll in seconds.</param>
    /// <param name="maxRollTime">Maximum time per digit roll in seconds.</param>
    /// <param name="variant">Full rolls each digit to completion; Fast reveals after a fixed time.</param>
    /// <returns>Coroutine enumerator.</returns>
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

        yield return StartCoroutine(GlowFade(_divergenceMeterMaterial, _defaultMaterialColor, _defaultMaterialColor * 20f, _glowCurve, 0.3f));

        AnimationEnded = true;
        GameData.SetSeed(Seed);
    }

    /// <summary>
    /// Idle loop that softly fades the meter's glow and updates each digit between pulses.
    /// </summary>
    /// <param name="time">Duration for each fade segment.</param>
    public IEnumerator IdleAnimation(float time = 1.5f)
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

    /// <summary>
    /// Linear material color lerp over a fixed duration.
    /// </summary>
    /// <param name="material">The material to adjust.</param>
    /// <param name="startMatColor">Start color.</param>
    /// <param name="targetMatColor">Target color.</param>
    /// <param name="time">Total duration of the fade.</param>
    /// <returns>Coroutine enumerator.</returns>
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

    /// <summary>
    /// Extracts a specific digit from a number for left-to-right meter display.
    /// </summary>
    /// <param name="number">The source number.</param>
    /// <param name="index">Zero-based digit index from left when <paramref name="length"/> is provided; otherwise from right.</param>
    /// <param name="length">If non-zero, total digit count used to align indices from the left.</param>
    /// <returns>The digit at the requested position, or 0 if out of bounds.</returns>
    public static int GetDigitFromNumber(int number, int index, int length = 0)
    {
        if (index > number.ToString().Length - 1 || length - index > number.ToString().Length)
        {
            return 0;
        }

        index = length == 0 ? index : length - index - 1;

        for (int i = 0; i < index; i++)
        {
            number /= 10;
        }

        return number % 10;
    }
}
