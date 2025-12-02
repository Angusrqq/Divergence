using System;
using System.Collections.Generic;

public class BaseAbilityScriptable : BaseScriptableObjectUnlockable
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;
    public virtual HandlerType Type => HandlerType.BaseAbility;
    public Utilities.AbilityTier Tier;
    public List<Action> upgradeActions;
}
