using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>MaskedGlowOnHover</c> class extends <c>GlowOnHoverButton</c> with mask support for proper stencil rendering.
/// </para>
/// <para>
/// Notifies the mask system when pointer events occur to ensure the glow effect renders correctly within masked UI elements.
/// </para>
/// </summary>
public class MaskedGlowOnHover : GlowOnHoverButton
{
    private Mask _mask;

    /// <summary>
    /// <para>
    /// <c>SetMask</c> method sets the mask component to notify when pointer events occur.
    /// </para>
    /// </summary>
    /// <param name="mask"><c>Mask</c> component to be notified of state changes</param>
    public void SetMask(Mask mask)
    {
        _mask = mask;
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerEnter</c> method applies the glow effect and notifies the mask system when the pointer enters the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerExit</c> method removes the glow effect and notifies the mask system when the pointer exits the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerDown</c> method notifies the mask system when the pointer is pressed down on the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }

    /// <summary>
    /// <para>
    /// <c>OnPointerUp</c> method notifies the mask system when the pointer is released from the button.
    /// </para>
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        //base.OnPointerUp(eventData);
        MaskUtilities.NotifyStencilStateChanged(_mask);
    }
}
