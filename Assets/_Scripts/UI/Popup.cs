using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;

    private RectTransform _rectTransform;
    private Vector3 _initialScale;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _startPosition;

    private Action<Vector3, Vector3, float> _scaleLerp => (start, end, t) => _rectTransform.localScale = Vector3.Lerp(start, end, _animationCurve.Evaluate(t));
    private Action<Quaternion, Quaternion, float> _rotationLerp => (start, end, t) => _rectTransform.rotation = Quaternion.Lerp(start, end, _animationCurve.Evaluate(t));
    private Action<Vector3, Vector3, float> _positionLerp => (start, end, t) => _rectTransform.position = Vector3.Lerp(start, end, _animationCurve.Evaluate(t));

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialScale = _rectTransform.localScale;
        _initialPosition = _rectTransform.position;
        _initialRotation = _rectTransform.rotation;

        _startPosition = new Vector3(-10, -10, 0);
    }

    // private async void OnDisable()
    // {
    //     if (_rectTransform == null) return;
    //     await AsyncAnimation(_rectTransform, 1f, _initialScale, Vector3.zero);
    // }

    public async void Disable()
    {
        if (_rectTransform == null) return;

        List<Task> tasks = new()
        {
            Utilities.AsyncAnimation(4f, _initialScale, Vector3.zero, _scaleLerp),
            Utilities.AsyncAnimation(4f, _initialPosition, _startPosition, _positionLerp)
        };

        await Task.WhenAll(tasks);
        gameObject.SetActive(false);
    }

    private async void OnEnable()
    {
        if (_rectTransform == null) return;

        List<Task> tasks = new()
        {
            Utilities.AsyncAnimation(4f, Vector3.zero, _initialScale, _scaleLerp),
            Utilities.AsyncAnimation(4f, _startPosition, _initialPosition, _positionLerp)
        };

        await Task.WhenAll(tasks);
        // await AsyncAnimation(4f, Vector3.zero, _initialScale, _scaleLerp);
        // await AsyncAnimation(4f, new Vector3(10f, 10f, 0f), _initialPosition, _positionLerp);
    }
}
