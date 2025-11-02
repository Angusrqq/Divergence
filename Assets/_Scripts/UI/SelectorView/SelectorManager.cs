using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>SelectorManager</c> is a class that manages a selection menu (Scroll View), such as character or map selection.
/// </para>
/// </summary>
public class SelectorManager : MonoBehaviour
{
    [NonSerialized] public ScriptableObject CurrentSelectedData;
    public TMPro.TMP_Text descriptionText;
    public Transform contentContainer;
    public Mask Mask;

    private SelectorItem _currentSelectedItem;

    public SelectorItem CurrentSelectedItem
    {
        get => _currentSelectedItem;
        set
        {
            _currentSelectedItem?.OnDeselect();
            _currentSelectedItem = value;
            CurrentSelectedData = _currentSelectedItem.ItemData;
        }
    }

    /// <summary>
    /// <para>
    /// <c>InitElements</c> instantiates and initializes the passed list of <c>SelectorItem</c> elements as children of <c>contentContainer</c>.
    /// </para>
    /// </summary>
    /// <param name="elements">Initial list of items to be instantiated</param>
    public void InitElements(List<SelectorItem> elements)
    {
        foreach (SelectorItem element in elements)
        {
            Instantiate(element, contentContainer);
        }
    }
    
    /// <summary>
    /// <c>UpdateDescription</c> updates the description text with the passed string.
    /// </summary>
    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
}
