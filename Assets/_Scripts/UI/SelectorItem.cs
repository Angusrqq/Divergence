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
    /// <param name="data"><c>ScriptableObject</c> to be associated with this item</param>
    /// <param name="manager"><c>SelectorManager</c> parent that will contain this item</param>
    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        selectorManager = manager;
        itemData = data;
    }
    
    /// <summary>
    /// <c>OnSelect</c> updates the currently selected item in the <c>SelectorManager</c>.
    /// </summary>
    public virtual void OnSelect(BaseEventData eventData)
    {
        selectorManager.currentSelectedItem = itemData;
    }
}
