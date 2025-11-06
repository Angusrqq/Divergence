using System;
using UnityEngine;

public class AbilityHandler : BaseAbilityHandler
{
    [NonSerialized] public float CooldownTimer;
    [NonSerialized] public float ActiveTimer;
    [NonSerialized] public AbilityState State;
    public bool CountActiveTimeInCooldown;
    public Stat ActiveTime;
    public Stat CooldownTime;
    public Stat KnockbackForce;
    public Stat KnockbackDuration;

    protected override void AfterInit()
    {
        Ability source = _source as Ability;
        State = AbilityState.ready;
        CountActiveTimeInCooldown = source.CountActiveTimeInCooldown;
        ActiveTime = source.ActiveTime;
        CooldownTime = source.CooldownTime;
        KnockbackForce = source.KnockbackForce;
        KnockbackDuration = source.KnockbackDuration;
    }

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

public enum HandlerType
{
    BaseAbility,
    Ability,
    InstantiatedAbility,
    Passive
}
