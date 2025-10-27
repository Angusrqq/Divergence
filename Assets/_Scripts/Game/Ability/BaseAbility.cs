using UnityEngine;

public class BaseAbility : ScriptableObject
{
    public string Name;
    public string Description;
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;
    public Sprite Icon;

    public virtual void Activate() { }

    public virtual void Upgrade()
    {
        Level++;
    }

    public virtual void UpdateAbility() { }
}
