using System;
using UnityEngine;

public class Duality : PassiveAbilityMono
{
    private StatModifier _damageModifier;
    private StatModifier _knockBackForceModifier;
    private float _cloneChance = 0.1f;

    public override void Activate()
    {
        _damageModifier = new StatModifier(-0.25f, StatModifierType.Percent, this);
        _knockBackForceModifier = new StatModifier(0.05f, StatModifierType.Percent, this);
        GameData.player.AbilityHolder.OnAbilityActivated += CloneProjectile;
    }


    private void CloneProjectile(Type type, InstantiatedAbilityHandler ability, InstantiatedAbilityMono projectile)
    {
        if(GameData.LowValue > _cloneChance) return;
        Vector2 initial_pos = (Vector2)GameData.player.transform.position + UnityEngine.Random.insideUnitCircle;
        var instance = Instantiate(projectile, initial_pos, Quaternion.identity);
        instance.Init(ability);
        instance.Damage.AddModifier(_damageModifier);
        instance.KnockbackForce.AddModifier(_knockBackForceModifier);
        instance.TryGetComponent<SpriteRenderer>(out var sr);
        if (sr != null)
        {
            sr.color = new Color(0f, 0f, 0.2f, 0.8f);
        }
        instance.OnDeath += OnProjectileDeath;
        ability.Instances.Add(instance);
    }

    public override void Upgrade()
    {
        _knockBackForceModifier.Value *= 2f;
        _damageModifier.Value += 0.05f;
        _cloneChance *= 1.3f;
    }

    private void OnProjectileDeath(InstantiatedAbilityMono instance)
    {
        instance.Damage.RemoveModifier(_damageModifier);
        instance.KnockbackForce.RemoveModifier(_knockBackForceModifier);
    }
}