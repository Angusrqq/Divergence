using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _killCounterText;
    private int _kills = 0;
    private float _killsPerSecond = 0f;
    private Material _iconMaterial;
    private RectTransform _iconTransform;
    private Color _initialColor;
    private List<float> _killTimes = new();
    private const float _KPSWINDOW = 3f;
    private float _shakeMagnitude = 0f;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        _iconTransform = transform.Find("Icon").GetComponent<RectTransform>();
        Image temp = _iconTransform.GetComponent<Image>();
        temp.material = Instantiate(temp.material);
        _iconMaterial = temp.material;

        _initialColor = Color.red;
    }

    private void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnEnemyDeath;
        _shakeCoroutine = StartCoroutine(ShakeIcon());
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _kills++;
        _killCounterText.text = _kills.ToString();

        float now = Time.time;
        _killTimes.Add(now);

        // Remove kills older than the window
        _killTimes.RemoveAll(t => t < now - _KPSWINDOW);

        // calculate KPS
        _killsPerSecond = _killTimes.Count / _KPSWINDOW;
        _shakeMagnitude = (_killsPerSecond - 25f) / 10f;

        Debug.Log($"Kills per second: {_killsPerSecond}");
        _iconMaterial.SetColor("_Color", _initialColor * (_killsPerSecond-30) * 10f);
    }

    private IEnumerator ShakeIcon()
    {
        Vector2 startPos = _iconTransform.localPosition;
        while (true)
        {
            if(_shakeMagnitude <= 0f) yield return null;
            else _iconTransform.localPosition = startPos + Random.insideUnitCircle * _shakeMagnitude;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.OnEnemyDeath -= OnEnemyDeath;
    }
}
