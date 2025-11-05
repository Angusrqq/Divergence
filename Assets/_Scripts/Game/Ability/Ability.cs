using System;
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
public class Ability : BaseAbility
{
    [NonSerialized] public float CooldownTimer;
    [NonSerialized] public float ActiveTimer;
    [NonSerialized] public AbilityState State = AbilityState.ready;
    public float CooldownTime;
    public float ActiveTime;
    public float KnockbackForce = 2f;
    public float KnockbackDuration = 0.25f;
    public bool CountActiveTimeInCooldown = true;

    /// <summary>
    /// <para>
    /// <c>Activate</c> is a virtual method that sets the state to active and starts the active timer.
    /// </para>
    /// </summary>
    public override void Activate()
    {
        State = AbilityState.active;
        if (CountActiveTimeInCooldown)
        {
            ActiveTimer = ActiveTime;
        }
    }

    /// <summary>
    /// <para>
    /// <c>StartCooldown</c> is a virtual method that sets the state to cooldown and starts the cooldown timer.
    /// </para>
    /// </summary>
    public virtual void StartCooldown()
    {
        State = AbilityState.cooldown;
        CooldownTimer = CooldownTime;
    }

    /// <summary>
    /// <para>
    /// <c>CooldownEnded</c> is a virtual method that sets the state to ready.
    /// </para>
    /// </summary>
    public virtual void CooldownEnded()
    {
        State = AbilityState.ready;
    }

    /// <summary>
    /// <para>
    /// Handles the states of the ability. Should be manually called in update.
    /// </para>
    /// </summary>
    public override void UpdateAbility()
    {
        switch (State)
        {
            case AbilityState.active:
                if (ActiveTimer > 0)
                {
                    ActiveTimer -= Time.deltaTime;
                }
                else
                {
                    StartCooldown();
                }
                break;

            case AbilityState.cooldown:
                if (CooldownTimer > 0)
                {
                    CooldownTimer -= Time.deltaTime;
                }
                else
                {
                    CooldownEnded();
                }
                break;
                
            case AbilityState.ready:
                Activate();
                break;
        }
    }

    /// <summary>
    /// <para>
    /// Handles the states of the ability to be used in fixed update.
    /// </para>
    /// </summary>
    public virtual void FixedUpdateAbility()
    {
        switch (State)
        {
            case AbilityState.active:
                if (ActiveTimer > 0)
                {
                    ActiveTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    StartCooldown();
                }
                break;

            case AbilityState.cooldown:
                if (CooldownTimer > 0)
                {
                    CooldownTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    CooldownEnded();
                }
                break;
                
            case AbilityState.ready:
                Activate();
                break;
        }
    }
}
