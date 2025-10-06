using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Example use of the ability system
/// </para>
/// Here we just instantiate the prefab and let it do the rest of the logic
/// </summary>
[CreateAssetMenu]
public class ThrowObjectTest : Ability
{
    [SerializeField] private TestAbility _prefab;
    private TestAbility _instance;
    public float speed;
    public float damage;

/// <summary>
/// <para>
/// This is the method that is called when the ability is activated
/// </para>
/// instantiates the prefab and sets the ability to it
/// </summary>
    public override void Activate()
    {
        _instance = Instantiate(_prefab, GameData.player.transform.position, Quaternion.identity);
        _instance.ability = this;
        base.Activate();
    }

/// <summary>
/// <para>
/// This is the method that is called when the ability is deactivated
/// </para>
/// destroys the instance of the prefab
/// </summary>
    public override void StartCooldown()
    {
        if (_instance) { Destroy(_instance.gameObject); _instance = null; }
        base.StartCooldown();
    }

    public void DestroyInstance()
    {
        Destroy(_instance.gameObject);
        _instance = null;
    }
}
