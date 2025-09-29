using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ThrowObjectTest : Ability
{
    [SerializeField] private TestAbility _prefab;
    private TestAbility _instance;
    public float speed;
    public float damage;

    public override void Activate()
    {
        _instance = Instantiate(_prefab, GameData.player.transform.position, Quaternion.identity);
        _instance.ability = this;
        base.Activate();
    }

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
