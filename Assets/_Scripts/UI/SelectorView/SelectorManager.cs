using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorManager : MonoBehaviour
{
    [NonSerialized] public ScriptableObject CurrentSelectedData;
    public TMPro.TMP_Text descriptionText;
    public Transform contentContainer;
    public Mask Mask;

    [Header("Scroll Settings")]
    public float scrollDuration = 0.25f; // Time to smooth scroll
    public float centerOffset = 0f; // Optional padding when centering

    private ScrollRect _scrollRect;
    private SelectorItem _currentSelectedItem;
    private Coroutine scrollRoutine;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public virtual SelectorItem CurrentSelectedItem
    {
        get => _currentSelectedItem;
        set
        {
            _currentSelectedItem?.OnDeselect();
            _currentSelectedItem = value;
            CurrentSelectedData = _currentSelectedItem.ItemData;

            ScrollToItem(_currentSelectedItem.transform as RectTransform);
        }
    }

    public virtual void InitElements(List<SelectorItem> elements)
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

    private void ScrollToItem(RectTransform target)
    {
        if (_scrollRect == null || target == null || gameObject.activeInHierarchy == false) return;

        // Stop previous smooth scroll
        if (scrollRoutine != null)
        {
            StopCoroutine(scrollRoutine);
        }

        scrollRoutine = StartCoroutine(SmoothScrollCoroutine(target));
    }

    private IEnumerator SmoothScrollCoroutine(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform content = _scrollRect.content;
        RectTransform viewport = _scrollRect.viewport;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        // World corners for checking visibility
        Vector3[] viewportCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);
        target.GetWorldCorners(targetCorners);

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        float targetTop = targetCorners[1].y;
        float targetBottom = targetCorners[0].y;

        // If fully visible, no need to scroll
        if (targetTop <= viewportTop && targetBottom >= viewportBottom) yield break;

        // ------------ Centering Logic ------------
        // Find the vertical offset in content local space
        float itemY = Mathf.Abs(target.anchoredPosition.y);
        float itemHeight = target.rect.height;

        // The Y position where the item should end up (center of viewport)
        float targetCenteredY = itemY - (viewportHeight / 2f) + (itemHeight / 2f) + centerOffset;

        // Normalize for ScrollRect
        float normalizedTarget = Mathf.Clamp01(targetCenteredY / (contentHeight - viewportHeight));
        float scrollTarget = 1f - normalizedTarget; // Invert because ScrollRect uses 1 = top

        // ------------ Smooth Scroll Animation ------------
        float start = _scrollRect.verticalNormalizedPosition;
        float t = 0f;

        while (t < scrollDuration)
        {
            t += Time.unscaledDeltaTime; // UI usually ignores game time scale
            float lerp = Mathf.SmoothStep(start, scrollTarget, t / scrollDuration);
            _scrollRect.verticalNormalizedPosition = lerp;

            yield return null;
        }

        _scrollRect.verticalNormalizedPosition = scrollTarget; // Ensure exact
    }
}
