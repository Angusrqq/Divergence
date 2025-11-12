using UnityEngine;

public class BaseScriptableObjectInfo : ScriptableObject
{
    [Header("Base Info")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    
    public string Name {get => _name; set => _name = value;}
    public string Description => _description;
    public Sprite Icon => _icon;

    // public override bool Equals(object other)
    // {
    //     BaseScriptableObjectInfo obj = (BaseScriptableObjectInfo)other;
    //     return obj != null && obj.Name == Name;
    // }

    // public override int GetHashCode()
    // {
    //     return Name.GetHashCode();
    // }
}
