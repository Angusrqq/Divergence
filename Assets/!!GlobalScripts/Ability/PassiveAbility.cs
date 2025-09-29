using UnityEngine;

public class PassiveAbility : ScriptableObject
{
    public new string name;
    public string description;
    public int level = 1;
    public int maxLevel;
    public PassiveAbilityType type;
    public virtual void Activate() { }
    public virtual void Upgrade(){}
    public virtual void UpdateAbility() { }
}

public enum PassiveAbilityType
{
    OneTime,
    Updated // dont know why but it will just exist for now
}
