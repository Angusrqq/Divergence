using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigationManager : MonoBehaviour
{
    public static UINavigationManager Instance { get; private set; }

    public bool autoFillSelection = true;

    private List<Transform> _activePanels = new();

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

        // Something is already selected
        if (EventSystem.current.currentSelectedGameObject != null)
            return;

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
            _activePanels.Add(panel);

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
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
    }

    private Button FindButton(Transform root)
    {
        foreach (Transform t in root)
        {
            if (!t.gameObject.activeInHierarchy)
                continue;

            Button b = t.GetComponent<Button>();
            if (b && b.interactable)
                return b;

            Button nested = FindButton(t);
            if (nested) return nested;
        }
        return null;
    }

    // -----------------------------------------------------------
    // Call this when you enable a panel: panel.SetActive(true);
    // -----------------------------------------------------------
    public void SelectFirstButtonInPanel(Transform panel)
    {
        StartCoroutine(DelaySelect(panel));
    }

    private System.Collections.IEnumerator DelaySelect(Transform panel)
    {
        yield return null; // wait 1 frame

        Button btn = FindFirstButton(panel);
        if (btn)
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
    }

    // -----------------------------------------------------------
    // INTERNAL HELPERS
    // -----------------------------------------------------------

    // Searches entire scene (not children of singleton!)
    private void SelectAnyButtonInScene()
    {
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var b in allButtons)
        {
            if (!b.gameObject.activeInHierarchy) continue;
            if (!b.interactable) continue;

            StartCoroutine(DelaySelect(b.transform));
            Debug.Log("Found and selected button: " + b.name);
            return;
        }
    }

    // Searches specific panel only
    private Button FindFirstButton(Transform root)
    {
        foreach (Transform t in root)
        {
            if (!t.gameObject.activeInHierarchy) continue;

            Button b = t.GetComponent<Button>();
            if (b && b.interactable)
                return b;

            // recurse
            Button nested = FindFirstButton(t);
            if (nested) return nested;
        }
        return null;
    }
}