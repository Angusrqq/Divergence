using UnityEngine;

public class BaseScriptableObjectUnlockable : BaseScriptableObjectInfo
{
    [Header("Unlockable Info")]
    [SerializeField] protected int _cost;
    [SerializeField] protected bool _isUnlocked = false;

    public int Cost => _cost;
    public bool IsUnlocked { get => _isUnlocked; set => _isUnlocked = value; }
}