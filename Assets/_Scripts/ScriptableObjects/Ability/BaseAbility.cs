using System;
using System.Collections.Generic;

public class BaseAbilityScriptable : BaseScriptableObjectInfo
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;
    public virtual HandlerType Type => HandlerType.BaseAbility;
    public List<Action> upgradeActions;
}
