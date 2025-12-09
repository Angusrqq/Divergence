using System;
using UnityEngine;

public class Berserk : PassiveAbilityMono
{
    private StatModifierByStat _damageModifier;
    private Stat _damageValue;
    private StatModifierByStat _speedModifier;
    private Stat _speedValue;
    private Stat _thresholdMult = 0.1f;
    private bool _isActive = false;

    void OnPlayerDamageTaken(UnityEngine.Object source, float amount, Type type = null)
    {
        if (GameData.player.DamageableEntity.Health <= GameData.player.DamageableEntity.MaxHealth * _thresholdMult && !_isActive)
        {
            _isActive = true;
            ActivateEffect();
        }
        else if (GameData.player.DamageableEntity.Health > GameData.player.DamageableEntity.MaxHealth * _thresholdMult && _isActive)
        {
            _isActive = false;
            DeactivateEffect();
        }
    }

    public override void Activate()
    {
        _damageValue = Ability.GetStat("Damage");
        _damageValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _damageModifier = new StatModifierByStat(ref _damageValue, StatModifierType.Mult, this);

        _speedValue = Ability.GetStat("Movement Speed");
        _speedValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _speedModifier = new StatModifierByStat(ref _speedValue, StatModifierType.Mult, this);

        GameData.player.DamageableEntity.OnDamageTaken += OnPlayerDamageTaken;
    }

    void ActivateEffect()
    {
        GameData.InGameAttributes.PlayerDamageMult.AddModifier(_damageModifier);
        GameData.player.MovementSpeed.AddModifier(_speedModifier);

        Debug.Log($"Berserk activated, player damage mult: {GameData.InGameAttributes.PlayerDamageMult}, damage bonus: {_damageValue}, player speed: {GameData.player.MovementSpeed}, speed bonus: {_speedValue}");
    }
    
    void DeactivateEffect()
    {
        GameData.InGameAttributes.PlayerDamageMult.RemoveModifier(_damageModifier);
        GameData.player.MovementSpeed.RemoveModifier(_speedModifier);
        
        Debug.Log($"Berserk deactivated, player damage mult: {GameData.InGameAttributes.PlayerDamageMult}, player speed: {GameData.player.MovementSpeed}");
    }
}
