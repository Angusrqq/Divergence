using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

/// <summary>
/// Controls the behavior of a 2D light, including fade in/out animations and timing sequences.
/// This class manages the light's radius changes using coroutines and customizable easing curves.
/// </summary>
public class LightControl : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light2D _spotLight;

    [Header("Timing Settings")]
    [SerializeField] private float _delayBeforeStart = 3f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _holdDuration = 1f;
    [SerializeField] private float _maxRadius = 8f;

    [Header("Easing Curve")]
    [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    /// <summary>
    /// Called when the script is initialized.
    /// Checks if the Light2D reference is set, and if not, logs an error message.
    /// If the reference is set, sets the point light outer radius to 0 and starts the light sequence coroutine.
    /// </summary>
    private void Start()
    {
        if (_spotLight == null)
        {
            Debug.LogError("LightControl: Light2D reference is missing!", this);
            return;
        }

        _spotLight.pointLightOuterRadius = 0f;
        StartCoroutine(LightSequence());
    }

    /// <summary>
    /// Coroutine for the light sequence.
    /// Waits for a specified amount of time, then fades the light in and out over a specified duration.
    /// Waits for a specified amount of time after the light has reached its maximum radius.
    /// </summary>
    /// <remarks>
    /// Calls the <see cref="ChangeRadius(float, float, float)"/> coroutine to change the radius of the light.
    /// </remarks>
    private IEnumerator LightSequence()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        yield return StartCoroutine(ChangeRadius(0f, _maxRadius, _fadeDuration));
        yield return new WaitForSeconds(_holdDuration);
        yield return StartCoroutine(ChangeRadius(_maxRadius, 0f, _fadeDuration));
    }

    /// <summary>
    /// Coroutine that changes the radius of the light over a specified duration.
    /// </summary>
    /// <param name="startValue">The starting value of the radius.</param>
    /// <param name="endValue">The ending value of the radius.</param>
    /// <param name="duration">The duration of the change in seconds.</param>
    /// <returns>An IEnumerator that can be used in a coroutine.</returns>
    private IEnumerator ChangeRadius(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = _curve.Evaluate(t);
            _spotLight.pointLightOuterRadius = Mathf.Lerp(startValue, endValue, easedT);
            yield return null;
        }

        _spotLight.pointLightOuterRadius = endValue;
    }
}
