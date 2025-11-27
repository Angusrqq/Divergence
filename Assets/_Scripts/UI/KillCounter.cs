using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _killCounterText;
    private int _kills = 0;

    private void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        Debug.Log("Enemy died");
        _kills++;
        _killCounterText.text = _kills.ToString();
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.OnEnemyDeath -= OnEnemyDeath;
    }
}
