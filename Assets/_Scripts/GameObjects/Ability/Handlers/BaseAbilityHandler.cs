using UnityEngine;

public class BaseAbilityHandler : MonoBehaviour
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;

    protected BaseAbilityScriptable _source;
    
    public string Name => _source.name;
    public string Description => _source.Description;
    public Sprite Icon => _source.Icon;

    public void Init(BaseAbilityScriptable source)
    {
        Level = source.Level;
        MaxLevel = source.MaxLevel;
        _source = source;
        IsEvolved = source.IsEvolved;
        AfterInit();
    }

    protected virtual void AfterInit()
    {
        
    }

    public virtual void Activate() { }

    public virtual void Upgrade()
    {
        Level++;
    }

    public virtual void UpdateAbility() { }
}
