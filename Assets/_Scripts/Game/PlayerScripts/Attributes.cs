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
    public static float health = 100;
    public static float maxHealth = 100;
    public static float playerDamageMult = 1f;
    public static float playerResistsMult = 1f;
    public static int projectilesAdd = 0;
    public static float castSpeedMult = 1f;
    public static float cooldownReductionMult = 1f;
    public static int activeAbilitySlots = 5;
    public static int passiveAbilitySlots = 5;
    public static int manuallyTriggeredAbilitySlots = 0;
    public static float passiveAbilityEffectMult = 1f;
    public static int pierceTargets = 0;
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
