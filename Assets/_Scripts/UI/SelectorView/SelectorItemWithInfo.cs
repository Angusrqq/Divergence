using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a selectable item in a UI selector with additional information display capabilities.
/// This class extends SelectorItem to provide enhanced functionality for displaying item details,
/// including icon, name, and description.
/// </summary>
public class SelectorItemWithInfo : SelectorItem
{
    [NonSerialized] public BaseScriptableObjectInfo Data;
    [NonSerialized] public TMPro.TMP_Text nameText;
    public Image ButtonImage;
    //public Color SelectColor = new(255, 225, 90, 255);

    private MaskedGlowOnHover _maskedGlowOnHover;
    //private Color _baseColor;

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

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SelectorManager.UpdateDescription(Data.Description);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
    }

    public virtual void SetUI()
    {
        ButtonImage.sprite = Data.Icon;
        nameText.text = Data.name;
    }
}
