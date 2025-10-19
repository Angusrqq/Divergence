using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// <para>
/// <c>SelectorItem</c> class represents an item in a selection menu (Scroll View), such as character or map selection.
/// </summary>
public class SelectorItem : MonoBehaviour, ISelectHandler
{
    public ScriptableObject itemData;
    public SelectorManager selectorManager;

    /// <summary>
    /// <para>
    /// <c>Init</c> method initializes the selector item with the given data and manager
    /// </para>
    /// </summary>
    /// <param name="data"></param> // TODO: Egor add desc for param
    /// <param name="manager"></param> // TODO: Egor add desc for param
    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        selectorManager = manager;
        itemData = data;
    }
    
    /// <summary>
    /// <c>OnSelect</c> updates the currently selected item in the <c>SelectorManager</c>.
    /// </summary>
    /// <param name="eventData"></param> // TODO: Egor add desc for param
    public virtual void OnSelect(BaseEventData eventData)
    {
        selectorManager.currentSelectedItem = itemData;
    }
}
