using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>GlowOnHoverButton</c> class adds a glowing effect to a button when hovered over or clicked.
/// </para>
/// <para>
/// Sets the glow effect on pointer events. Acts as a component for any button with a <c>TMP_Text</c> child.
/// </para>
/// </summary>
[RequireComponent(typeof(Button))]
public class GlowOnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    protected TMP_Text _tmp;
    protected Button _button;
    protected Color _defaultColor;

    /// <summary>
    /// <para>
    /// <c>Start</c> method initializes the button and text components, and stores the default glow color.
    /// </para>
    /// </summary>
    public virtual void Start()
    {
        _button = GetComponent<Button>();
        _tmp = GetComponentInChildren<TMP_Text>();
        _defaultColor = _tmp.fontMaterial.GetColor("_GlowColor");
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerEnter</c> method applies the glow effect when the pointer enters the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerExit</c> method removes the glow effect when the pointer exits the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerDown</c> method applies a pressed glow effect when the pointer is pressed down on the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", 1f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerUp</c> method removes the glow effect when the pointer is released from the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    public static IEnumerator MaterialFloatPropertyLerp(Material material, string propertyName, float speed, AnimationCurve curve = null, float from = 0f, float to = 1f)
    {
        float timePassed = 0f;
        if (curve != null)
        {
            while (timePassed <= 1f)
            {
                timePassed += Time.deltaTime * speed;
                material.SetFloat(propertyName, curve.Evaluate(timePassed));
                yield return null;
            }
        }
        else
        {
            while (timePassed <= 1f)
            {
                timePassed += Time.deltaTime * speed;
                material.SetFloat(propertyName, Mathf.Lerp(from, to, timePassed));
                yield return null;
            }
        }
    }
}
