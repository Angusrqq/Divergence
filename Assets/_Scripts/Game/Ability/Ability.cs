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
public class Ability : ScriptableObject
{
    public new string name;
    public string description;
    public float cooldownTime;
    public int level = 1;
    public int maxLevel;
    public float activeTime;
    public float knockbackForce = 2f;
    public float knockbackDuration = 0.25f;
    public bool isEvolved { get; private set; } = false; //TODO: change to nonserialized (its not serialized for testing purposes)
    [NonSerialized] public float cooldownTimer;
    [NonSerialized] public float activeTimer;
    [NonSerialized] public AbilityState state = AbilityState.ready;

    /// <summary>
    /// <para>
    /// <c>Activate</c> is a virtual method that sets the state to active and starts the active timer.
    /// </para>
    /// </summary>
    public virtual void Activate()
    {
        state = AbilityState.active;
        activeTimer = activeTime;
    }

    /// <summary>
    /// <para>
    /// <c>StartCooldown</c> is a virtual method that sets the state to cooldown and starts the cooldown timer.
    /// </para>
    /// </summary>
    public virtual void StartCooldown()
    {
        state = AbilityState.cooldown;
        cooldownTimer = cooldownTime;
    }

    /// <summary>
    /// <para>
    /// <c>CooldownEnded</c> is a virtual method that sets the state to ready.
    /// </para>
    /// </summary>
    public virtual void CooldownEnded()
    {
        state = AbilityState.ready;
    }

    /// <summary>
    /// <para>
    /// <c>Upgrade</c> is a virtual method that upgrades the ability.
    /// </para>
    /// </summary>
    public virtual void Upgrade() { }

    /// <summary>
    /// <para>
    /// Handles the states of the ability. Should be manually called in update.
    /// </para>
    /// </summary>
    public virtual void UpdateAbility()
    {
        switch (state)
        {
            case AbilityState.active:
                if (activeTimer > 0)
                {
                    activeTimer -= Time.deltaTime;
                }
                else
                {
                    StartCooldown();
                }
                break;

            case AbilityState.cooldown:
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
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
        switch (state)
        {
            case AbilityState.active:
                if (activeTimer > 0)
                {
                    activeTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    StartCooldown();
                }
                break;

            case AbilityState.cooldown:
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.fixedDeltaTime;
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
