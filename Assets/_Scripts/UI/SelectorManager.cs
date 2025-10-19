using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>SelectorManager</c> is a class that manages a selection menu (Scroll View), such as character or map selection.
/// </para>
/// </summary>
public class SelectorManager : MonoBehaviour
{
    [NonSerialized] public ScriptableObject currentSelectedItem;
    public TMPro.TMP_Text descriptionText;
    public Transform contentContainer;

    /// <summary>
    /// <para>
    /// <c>InitElements</c> instantiates and initializes the passed list of <c>SelectorItem</c> elements as children of <c>contentContainer</c>.
    /// </para>
    /// </summary>
    /// <param name="elements"></param> // TODO: Egor add desc for param
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
    /// <param name="description"></param> // TODO: Egor add desc for param
    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
}
