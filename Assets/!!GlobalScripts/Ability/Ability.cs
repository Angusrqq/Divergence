using System;
using UnityEngine.UI;
using UnityEngine;
//TODO: EVGENIY READ THIS
// IF YOU DONT KNOW WHAT IS A SCRIPTABLE OBJECT
// /\ read this: https://learn.unity.com/tutorial/introduction-to-scriptable-objects
// watch this: https://www.youtube.com/watch?v=ry4I6QyPw4E (this is what i watched when making this)

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
    [NonSerialized] public float cooldownTimer;
    [NonSerialized] public float activeTimer;
    [NonSerialized] public AbilityState state = AbilityState.ready;

    public virtual void Activate()
    {
        state = AbilityState.active;
        activeTimer = activeTime;
    }

    public virtual void StartCooldown()
    {
        state = AbilityState.cooldown;
        cooldownTimer = cooldownTime;
    }

    public virtual void CooldownEnded()
    {
        state = AbilityState.ready;
    }

    public virtual void Upgrade() { }

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
