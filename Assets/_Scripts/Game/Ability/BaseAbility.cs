public class BaseAbility : BaseScriptableObjectInfo
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;

    public virtual void Activate() { }

    public virtual void Upgrade()
    {
        Level++;
    }

    public virtual void UpdateAbility() { }
}
