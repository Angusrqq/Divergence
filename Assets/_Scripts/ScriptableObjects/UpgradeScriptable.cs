using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeScriptable : BaseScriptableObjectUnlockable
{
    [Header("Upgrade Info")]
    [SerializeField] private int _level = 0;
    [SerializeField] private int _maxLevel = 5;
    [SerializeField] private List<int> _upgradeCosts;
    public int Level {get => _level; set => _level = value; }
    public int MaxLevel {get => _maxLevel; set => _maxLevel = value; }
    public List<int> UpgradeCosts {get => _upgradeCosts; set => _upgradeCosts = value; }
    public UpgradeLogicSO UpgradeLogic;
}
//TODO: Decide if we really need this, if so move it to a different file
// public class Unlockable : BaseScriptableObjectInfo
// {
//     private bool _unlocked = false;
//     public bool Unlocked => _unlocked;
//     public Predicate<Unlockable> UnlockCondition { get; set; }
//     public virtual void OnUnlock() { }
// }