
using System;
using UnityEngine;

public class Shrapnel : PassiveAbilityMono
{
    public ShrapnelInstance Prefab;
    private float _lastHealth;
    private float _healthThreshold = 20f;
    private float _buffer = 0f;
    private float _radius = 5f;
    [NonSerialized] public Stat Damage = 20f;
    [NonSerialized] public Stat Speed = 0.4f;
    [NonSerialized] public byte _localProjectiles = 5;
    private static ShrapnelInstance _instance;

    private const byte SPRITE_OFFSET = 45;

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
        for(int i = 0; i < _localProjectiles + GameData.InGameAttributes.ProjectilesAdd; i++)
        {
            float instanceCircleRotation = 360f / (_localProjectiles + GameData.InGameAttributes.ProjectilesAdd) * i;
            _instance = Instantiate(Prefab, GameData.player.transform.position, Quaternion.AngleAxis(instanceCircleRotation + SPRITE_OFFSET, Vector3.forward));
            Vector2 direction = new Vector2(Mathf.Cos(instanceCircleRotation * Mathf.Deg2Rad), Mathf.Sin(instanceCircleRotation * Mathf.Deg2Rad));
            _instance.Init(damage, _radius, direction, Speed);
        }
        
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
        _localProjectiles += 2;
    }
}
