
using System;
using UnityEngine;

public class Shrapnel : PassiveAbilityMono
{
    public ShrapnelInstance Prefab;
    private float _lastHealth;
    private float _buffer = 0f;
    [NonSerialized] public Stat Damage = 20f;
    [NonSerialized] public Stat Speed = 1f;

    private const byte SPRITE_OFFSET = 45;

    void OnPlayerDamageTaken(UnityEngine.Object source, float amount, Type type = null)
    {
        if(amount >= Ability.GetStat("Required damage") - _buffer)
        {
            _lastHealth = GameData.player.DamageableEntity.Health;
            ActivateEffect(Damage + _buffer);
            _buffer = 0f;
        } 
        else _buffer += amount;
    }

    private void ActivateEffect(float damage)
    {
        for(int i = 0; i < Ability.GetStat("Thorns released") + GameData.InGameAttributes.ProjectilesAdd; i++)
        {
            float instanceCircleRotation = 360f / (Ability.GetStat("Thorns released") + GameData.InGameAttributes.ProjectilesAdd) * i;
            var _instance = Instantiate(Prefab, GameData.player.transform.position, Quaternion.AngleAxis(instanceCircleRotation + SPRITE_OFFSET, Vector3.forward));
            Vector2 direction = new Vector2(Mathf.Cos(instanceCircleRotation * Mathf.Deg2Rad), Mathf.Sin(instanceCircleRotation * Mathf.Deg2Rad));
            _instance.Init(damage, Ability.GetStat("Thorn size"), direction, Speed);
        }
        
    }

    public override void Activate()
    {
        Damage.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        Damage.AddModifier(GameData.InGameAttributes.PlayerDamageMultModifier);
        GameData.player.DamageableEntity.OnDamageTaken += OnPlayerDamageTaken;
    }

    // public override void Upgrade()
    // {
    //     _healthThreshold -= 5f;
    //     _radius += 1f;
    //     _localProjectiles += 2;
    // }
}
