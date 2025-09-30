using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectorItem : MonoBehaviour, ISelectHandler
{
    public ScriptableObject itemData;
    public SelectorManager selectorManager;

    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        selectorManager = manager;
        itemData = data;
    }
    public virtual void OnSelect(BaseEventData eventData)
    {
        selectorManager.currentSelectedItem = itemData;
    }
}
