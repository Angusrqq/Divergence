using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GlowOnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    protected TMP_Text _tmp;
    protected Button _button;
    protected Color _defaultColor;

    protected void Start()
    {
        _button = GetComponent<Button>();

        _tmp = GetComponentInChildren<TMP_Text>();
        _tmp.fontMaterial = new(_tmp.fontMaterial);
        _defaultColor = _tmp.fontMaterial.GetColor("_GlowColor");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        EventSystem.current.SetSelectedGameObject(gameObject);
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", 1f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        if (!_button.interactable) return;
        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    protected void OnDisable()
    {
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
