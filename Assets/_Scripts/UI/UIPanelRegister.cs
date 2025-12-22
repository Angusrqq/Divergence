using UnityEngine;

public class UIPanelRegister : MonoBehaviour
{
    private void OnEnable()
    {
        if (UINavigationManager.Instance != null)
        {
            UINavigationManager.Instance.RegisterPanel(transform);
        }
    }

    private void OnDisable()
    {
        if (UINavigationManager.Instance != null)
        {
            UINavigationManager.Instance.UnregisterPanel(transform);
        }
    }
}
