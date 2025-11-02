using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents an item in a selection menu (Scroll View), such as character or map selection.
/// Handles selection state updates and visual feedback through a highlighted image.
/// </summary>
public class SelectorItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] private ScriptableObject _itemData;
    [SerializeField] private SelectorManager _selectorManager;

    public Image SelectedImage;

    public ScriptableObject ItemData => _itemData;
    public SelectorManager SelectorManager
    {
        get => _selectorManager;
        set => _selectorManager = value;
    }

    /// <summary>
    /// Initializes the selector item with the given data and manager.
    /// </summary>
    /// <param name="data">The <c>ScriptableObject</c> to be associated with this item.</param>
    /// <param name="manager">The <c>SelectorManager</c> parent that will contain this item.</param>
    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        _selectorManager = manager;
        _itemData = data;
    }

    /// <summary>
    /// Called when this item is selected. Updates the SelectorManager's current selection and enables the selection image.
    /// </summary>
    /// <param name="eventData">The event data associated with the selection.</param>
    public virtual void OnSelect(BaseEventData eventData)
    {
        _selectorManager.CurrentSelectedItem = this;
        SelectedImage.enabled = true;
    }

    /// <summary>
    /// Called when this item is deselected. Disables the selection image to indicate the item is no longer selected.
    /// This method is invoked by the <see cref="SelectorManager"/> when a new item is selected.
    /// </summary>
    public virtual void OnDeselect()
    {
        SelectedImage.enabled = false;
    }
}
