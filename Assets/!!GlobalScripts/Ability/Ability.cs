using System;
using UnityEngine.UI;
using UnityEngine;

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

    public virtual void Upgrade(){}

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
