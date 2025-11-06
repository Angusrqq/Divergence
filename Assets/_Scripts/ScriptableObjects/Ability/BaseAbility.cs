using System;

public class BaseAbilityScriptable : BaseScriptableObjectInfo
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;
    public virtual HandlerType Type => HandlerType.BaseAbility;

    public virtual void Activate() { }

    public virtual void Upgrade()
    {
        Level++;
    }

    public virtual void UpdateAbility() { }
}
