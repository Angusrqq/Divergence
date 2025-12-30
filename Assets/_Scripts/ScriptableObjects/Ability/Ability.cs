using UnityEngine;

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
