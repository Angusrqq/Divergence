using UnityEngine;

/// <summary>
/// <para>
/// <c>AbilityState</c> enum represents the different states an ability can be in.
/// </para>
/// <c>Ability</c> is a ScriptableObject that represents an ability that can be used by a player or an enemy.
/// <para>
/// It has properties like name, description, cooldown time, level, max level, active time, knockback force, and knockback duration.
/// It also has methods to activate the ability, start the cooldown, handle the end of the cooldown, upgrade the ability, and update the ability state.
/// </para>
/// </summary>
public class Ability : BaseAbilityScriptable
{
    [Header("Ability Properties")]
    public float CooldownTime;
    public float ActiveTime;
    public float KnockbackForce = 2f;
    public float KnockbackDuration = 0.25f;
    public bool CountActiveTimeInCooldown = true;

    public override HandlerType Type => HandlerType.Ability;
}
