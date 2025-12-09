using System;
using UnityEngine;

public class Duality : PassiveAbilityMono
{
    private StatModifier _damageModifier;
    private StatModifier _knockBackForceModifier;
    private Stat _cloneDamage;
    private Stat _knockbackForce;

    public override void Activate()
    {
        _cloneDamage = Ability.GetStat("Clone Damage");
        _knockbackForce = Ability.GetStat("Knockback Force");

        _damageModifier = new StatModifierByStat(ref _cloneDamage, StatModifierType.Percent, this);
        _knockBackForceModifier = new StatModifierByStat(ref _knockbackForce, StatModifierType.Percent, this);
        GameData.player.AbilityHolder.OnAbilityActivated += CloneProjectile;
    }


    private void CloneProjectile(Type type, InstantiatedAbilityHandler ability, InstantiatedAbilityMono projectile)
    {
        if (GameData.LowValue > Ability.GetStat("Clone Chance")) return;
        Vector2 initial_pos = (Vector2)GameData.player.transform.position + UnityEngine.Random.insideUnitCircle;

        var instance = Instantiate(projectile, initial_pos, Quaternion.identity);
        instance.Init(ability);
        instance.Ability.GetStat("Damage").AddModifier(_damageModifier);
        instance.Ability.GetStat("Knockback Force").AddModifier(_knockBackForceModifier);
        instance.TryGetComponent<SpriteRenderer>(out var sr);

        if (sr != null)
        {
            sr.color = new Color(0f, 0f, 0.2f, 0.8f);
        }

        instance.OnDeath += OnProjectileDeath;
        ability.Instances.Add(instance);
    }

    private void OnProjectileDeath(InstantiatedAbilityMono instance)
    {
        instance.Ability.GetStat("Damage").RemoveModifier(_damageModifier);
        instance.Ability.GetStat("Knockback Force").RemoveModifier(_knockBackForceModifier);
    }
}
