using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorSwitcher : MonoBehaviour
{
    [SerializeField] private SelectorManager _defaultSelectorManager;
    
    private SelectorManager _currentSelectorManager;

    public void OnButtonClicked(SelectorManager manager)
    {
        if (_currentSelectorManager == manager) return;

        if (_currentSelectorManager != null)
        {
            _currentSelectorManager.transform.parent.gameObject.SetActive(false);
        }

        _currentSelectorManager = manager;
        _currentSelectorManager.CurrentSelectedItem.OnSelect(new PointerEventData(EventSystem.current));
        _currentSelectorManager.transform.parent.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        if (_currentSelectorManager != null)
        {
            _currentSelectorManager.transform.parent.gameObject.SetActive(false);
        }
    }

    public void Activate()
    {
        if (_currentSelectorManager != null)
        {
            _currentSelectorManager.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            _currentSelectorManager = _defaultSelectorManager;
            _currentSelectorManager.transform.parent.gameObject.SetActive(true);
        }
    }
}
