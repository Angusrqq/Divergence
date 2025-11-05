using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class Upgrade : BaseScriptableObjectInfo
{
    private int _level = 0;
    private int _cost = 0;
    public int Level => _level;
    public int Cost => _cost;
    public virtual void OnBuy() { }
}
//TODO: Decide if we really need this, if so move it to a different file
public class Unlockable : BaseScriptableObjectInfo
{
    private bool _unlocked = false;
    public bool Unlocked => _unlocked;
    public Predicate<Unlockable> UnlockCondition { get; set; }
    public virtual void OnUnlock() { }
}