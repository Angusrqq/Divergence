using System;
using UnityEngine;

public class Duality : PassiveAbilityMono
{
    private StatModifier _damageModifier;
    private StatModifier _knockBackForceModifier;

    public override void Activate()
    {
        _damageModifier = new StatModifier(-0.25f, StatModifierType.Percent, this);
        _knockBackForceModifier = new StatModifier(0.05f, StatModifierType.Percent, this);
        GameData.player.AbilityHolder.OnAbilityActivated += CloneProjectile;
    }


    private void CloneProjectile(Type type, InstantiatedAbilityHandler ability, InstantiatedAbilityMono projectile)
    {
        Vector2 initial_pos = (Vector2)GameData.player.transform.position + UnityEngine.Random.insideUnitCircle;
        var instance = Instantiate(projectile, initial_pos, Quaternion.identity);
        instance.Init(ability);
        instance.Damage.AddModifier(_damageModifier);
        instance.KnockbackForce.AddModifier(_knockBackForceModifier);
        instance.TryGetComponent<SpriteRenderer>(out var sr);
        if (sr != null)
        {
            sr.color = Color.blue;
        }
        instance.OnDeath += OnProjectileDeath;
        ability.Instances.Add(instance);
    }

    public override void Upgrade()
    {
        _knockBackForceModifier.Value *= 2f;
    }

    private void OnProjectileDeath(InstantiatedAbilityMono instance)
    {
        instance.Damage.RemoveModifier(_damageModifier);
        instance.KnockbackForce.RemoveModifier(_knockBackForceModifier);
    }
}