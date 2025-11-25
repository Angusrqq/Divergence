
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Shrapnel : PassiveAbilityMono
{
    public ShrapnelInstance Prefab;
    private float _lastHealth;
    private float _healthThreshold = 20f;
    private float _buffer = 0f;
    private float _radius = 5f;
    [NonSerialized] public Stat Damage = 20f;
    private static ShrapnelInstance _instance;

    void OnPlayerDamageTaken(UnityEngine.Object source, float amount, Type type = null)
    {
        if(_instance != null) return;
        if(amount >= _lastHealth - (_healthThreshold - _buffer) || amount >= _healthThreshold - _buffer)
        {
            _lastHealth = GameData.player.DamageableEntity.Health;
            ActivateEffect(Damage + _buffer);
            _buffer = 0f;
        } 
        else _buffer += amount;
    }

    private void ActivateEffect(float damage)
    {
        _instance = Instantiate(Prefab, GameData.player.transform.position, Quaternion.identity);
        _instance.Init(damage, _radius);
    }

    public override void Activate()
    {
        Damage.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        Damage.AddModifier(GameData.InGameAttributes.PlayerDamageMultModifier);
        GameData.player.DamageableEntity.OnDamageTaken += OnPlayerDamageTaken;
    }

    public override void Upgrade()
    {
        _healthThreshold -= 5f;
        _radius += 1f;
    }
}
