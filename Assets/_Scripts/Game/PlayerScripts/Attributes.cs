using UnityEngine;
using System;

public enum AttributeId
{
    MagnetRadius
}

/// <summary>
/// <para>
/// <c>Attributes</c> is a static class that contains the attributes of the player.
/// </para>
/// </summary>
public static class Attributes
{
    public static float Health = 100;
    public static float MaxHealth = 100;
    public static float PlayerDamageMult = 1f;
    public static float PlayerResistsMult = 1f;
    public static int ProjectilesAdd = 0;
    public static float CastSpeedMult = 1f;
    public static float CooldownReductionMult = 1f;
    public static int ActiveAbilitySlots = 5;
    public static int AbilitiesPerLevel = 3;
    public static int PassiveAbilitySlots = 5;
    public static int ManuallyTriggeredAbilitySlots = 0;
    public static float PassiveAbilityEffectMult = 1f;
    public static int PierceTargets = 0;
    public static event Action<AttributeId, float> OnAttributeChanged;

    private static float _magnetRadius = 0.5f;

    public static float MagnetRadius
    {
        get => _magnetRadius;
        set
        {
            if (Mathf.Approximately(_magnetRadius, value)) return;
            _magnetRadius = value;
            OnAttributeChanged?.Invoke(AttributeId.MagnetRadius, _magnetRadius);
        }
    }
}
