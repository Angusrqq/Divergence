using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a selectable item in a UI selector with additional information display capabilities.
/// This class extends SelectorItem to provide enhanced functionality for displaying item details,
/// including icon, name, and description when selected or hovered.
/// </summary>
public class SelectorItemWithInfo : SelectorItem
{
    [NonSerialized] public BaseScriptableObjectInfo Data;
    [NonSerialized] public TMPro.TMP_Text nameText;
    public Image ButtonImage;
    //public Color SelectColor = new(255, 225, 90, 255);

    private MaskedGlowOnHover _maskedGlowOnHover;
    //private Color _baseColor;

    /// <summary>
    /// <para>
    /// <c>Init</c> method initializes the data button with the given data and selector manager.
    /// </para>
    /// <para>
    /// Sets up the masked glow component with the selector manager's mask.
    /// </para>
    /// </summary>
    /// <param name="data"><c>Data</c> to be associated with this button</param>
    /// <param name="manager"><c>SelectorManager</c> parent that will contain this button</param>
    public virtual void Init(BaseScriptableObjectInfo data, SelectorManager manager)
    {
        nameText = GetComponentInChildren<TMPro.TMP_Text>();
        Data = data;
        base.Init(Data, manager);
        SelectorManager = manager;
        _maskedGlowOnHover = GetComponent<MaskedGlowOnHover>();
        _maskedGlowOnHover.SetMask(SelectorManager.Mask);
        SetUI();
    }

    /// <summary>
    /// <para>
    /// <c>OnSelect</c> method updates the description in the selector manager when this button is selected.
    /// </para>
    /// </summary>
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SelectorManager.UpdateDescription(Data.Description);
    }

    /// <summary>
    /// <para>
    /// <c>OnDeselect</c> method hides the border image when this button is deselected.
    /// </para>
    /// </summary>
    public override void OnDeselect()
    {
        base.OnDeselect();
    }

    /// <summary>
    /// <para>
    /// <c>SetUI</c> method sets the button image and name text based on whether the character is unlocked or not.
    /// </para>
    /// </summary>
    public virtual void SetUI()
    {
        ButtonImage.sprite = Data.Icon;
        nameText.text = Data.name;
    }
}
