using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseScriptableObjectInfo : ScriptableObject
{
    [Header("Base Info")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [HideInInspector] public string Guid;
    
    public string Name {get => _name; set => _name = value;}
    public string Description => _description;
    public Sprite Icon => _icon;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(Guid))
        {
            Guid = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif

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
