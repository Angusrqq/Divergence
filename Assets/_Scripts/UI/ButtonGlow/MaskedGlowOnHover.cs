using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaskedGlowOnHover : GlowOnHoverButton
{
    private Mask _mask;

    public void SetMask(Mask mask)
    {
        _mask = mask;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;

        _tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        _tmp.fontMaterial.SetFloat("_GlowPower", 1f);
        
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        MaskUtilities.NotifyStencilStateChanged(_mask);
    }
}
