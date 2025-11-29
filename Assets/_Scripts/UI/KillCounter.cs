using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _killCounterText;
    private int _kills = 0;
    private float _timer = 0f;
    private float _killsPerSecond = 0f;
    private int _lastKillsValueBeforeUpdate = 0;
    private Material _iconMaterial;
    private Color _initialColor;

    private void Awake()
    {
        Image temp = transform.Find("Icon").GetComponent<Image>();
        temp.material = Instantiate(temp.material);
        _iconMaterial = temp.material;

        _initialColor = Color.red;
    }

    private void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _kills++;
        _killCounterText.text = _kills.ToString();
        // Debug.Log($"Time passed: {timePassed}");
        // Debug.Log($"1/kills * timePassed: {1/(_kills-1f)}");
    }

    //TODO: figure out how to get kills per second on enemy death instead of over a period of time

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1f)
        {
            _killsPerSecond = _kills -_lastKillsValueBeforeUpdate / _timer;
            _lastKillsValueBeforeUpdate = _kills;
            _timer = 0f;
            Debug.Log($"Kills per second: {_killsPerSecond}");
            _iconMaterial.SetColor("_Color", _initialColor * _killsPerSecond);
        }
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.OnEnemyDeath -= OnEnemyDeath;
    }
}
