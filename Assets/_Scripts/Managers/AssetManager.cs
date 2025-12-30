using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public DamagePopup damagePopupPrefab;
    public DamagePopup criticalDamagePopupPrefab;

    public static AssetManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
}
