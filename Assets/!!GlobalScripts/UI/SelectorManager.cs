using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    [NonSerialized] public ScriptableObject currentSelectedItem;
    public TMPro.TMP_Text descriptionText;
    public Transform contentContainer;
    
    public void InitElements(List<SelectorItem> elements)
    {
        foreach (SelectorItem element in elements)
        {
            Instantiate(element, contentContainer);
        }
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
}
