using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// <para>
/// <c>SelectorItem</c> class represents an item in a selection menu (Scroll View), such as character or map selection.
/// </summary>
public class SelectorItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] private ScriptableObject _itemData;
    [SerializeField] private SelectorManager _selectorManager;
    public ScriptableObject ItemData => _itemData;
    public SelectorManager SelectorManager
    {
        get => _selectorManager;
        set => _selectorManager = value;
    }


    /// <summary>
    /// <para>
    /// <c>Init</c> method initializes the selector item with the given data and manager
    /// </para>
    /// </summary>
    /// <param name="data"><c>ScriptableObject</c> to be associated with this item</param>
    /// <param name="manager"><c>SelectorManager</c> parent that will contain this item</param>
    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        _selectorManager = manager;
        _itemData = data;
    }

    /// <summary>
    /// <c>OnSelect</c> updates the currently selected item in the <c>SelectorManager</c>.
    /// </summary>
    public virtual void OnSelect(BaseEventData eventData)
    {
        _selectorManager.CurrentSelectedItem = this;
    }

    public virtual void OnDeselect() { }
}
