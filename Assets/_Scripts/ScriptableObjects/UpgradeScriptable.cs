using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeScriptable : BaseScriptableObjectUnlockable
{
    [Header("Upgrade Info")]
    [SerializeField] private int _level = 0;
    [SerializeField] private int _maxLevel = 5;
    [SerializeField] private List<int> _upgradeCosts;

    public UpgradeLogicSO UpgradeLogic;

    public int Level
    {
        get => _level;
        set => _level = value;
    }
    public int MaxLevel
    {
        get => _maxLevel;
        set => _maxLevel = value;
    }
    public List<int> UpgradeCosts
    {
        get => _upgradeCosts;
        set => _upgradeCosts = value;
    }
}
