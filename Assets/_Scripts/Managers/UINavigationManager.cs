using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigationManager : MonoBehaviour
{
    public bool autoFillSelection = true;

    private List<Transform> _activePanels = new();

    public static UINavigationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!autoFillSelection) return;
        if (EventSystem.current.currentSelectedGameObject != null) return; // Something is already selected
        if (_activePanels.Count <= 0) return;

        // Try selecting from active panels first
        for (int i = _activePanels.Count - 1; i >= 0; i--)
        {
            Transform panel = _activePanels[i];
            if (panel != null && panel.gameObject.activeInHierarchy)
            {
                SelectFirstButton(panel);
                return;
            }
        }
    }

    public void RegisterPanel(Transform panel)
    {
        if (!_activePanels.Contains(panel))
        {
            _activePanels.Add(panel);
        }

        // Delay 1 frame to ensure layout is built
        StartCoroutine(SelectDelayed(panel));
    }

    public void UnregisterPanel(Transform panel)
    {
        _activePanels.Remove(panel);
    }

    private System.Collections.IEnumerator SelectDelayed(Transform panel)
    {
        yield return null;
        SelectFirstButton(panel);
    }

    private void SelectFirstButton(Transform panel)
    {
        Button btn = FindButton(panel);
        if (btn != null)
        {
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
        }
    }

    private Button FindButton(Transform root)
    {
        foreach (Transform transform in root)
        {
            if (!transform.gameObject.activeInHierarchy) continue;

            Button btn = transform.GetComponent<Button>();
            if (btn && btn.interactable) return btn;

            Button nested = FindButton(transform);
            if (nested) return nested;
        }
        
        return null;
    }

    // Call this when you enable a panel: panel.SetActive(true);
    public void SelectFirstButtonInPanel(Transform panel)
    {
        StartCoroutine(DelaySelect(panel));
    }

    private System.Collections.IEnumerator DelaySelect(Transform panel)
    {
        yield return null; // Wait 1 frame

        Button btn = FindFirstButton(panel);
        if (btn)
        {
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
        }
    }

    // Searches entire scene (not children of singleton!)
    private void SelectAnyButtonInScene()
    {
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var btn in allButtons)
        {
            if (!btn.gameObject.activeInHierarchy) continue;
            if (!btn.interactable) continue;

            StartCoroutine(DelaySelect(btn.transform));

            Debug.Log("Found and selected button: " + btn.name);
            return;
        }
    }

    // Searches specific panel only
    private Button FindFirstButton(Transform root)
    {
        foreach (Transform transform in root)
        {
            if (!transform.gameObject.activeInHierarchy) continue;

            Button btn = transform.GetComponent<Button>();
            if (btn && btn.interactable) return btn;

            Button nested = FindFirstButton(transform);
            if (nested) return nested;
        }

        return null;
    }
}
