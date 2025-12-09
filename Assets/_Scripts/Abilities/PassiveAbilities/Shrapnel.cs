using System;
using UnityEngine;

public class Shrapnel : PassiveAbilityMono
{
    private const byte SPRITE_OFFSET = 45;

    [NonSerialized] public Stat Damage = 20f;
    [NonSerialized] public Stat Speed = 1f;
    public ShrapnelInstance Prefab;

    private float _lastHealth;
    private float _buffer = 0f;

    void OnPlayerDamageTaken(UnityEngine.Object source, float amount, Type type = null)
    {
        if (amount >= Ability.GetStat("Required Damage") - _buffer)
        {
            _lastHealth = GameData.player.DamageableEntity.Health;
            ActivateEffect(Damage + _buffer);
            _buffer = 0f;
        } 
        else
        {
            _buffer += amount;
        }
    }

    private void ActivateEffect(float damage)
    {
        for (int i = 0; i < Ability.GetStat("Thorns Released") + GameData.InGameAttributes.ProjectilesAdd; i++)
        {
            float instanceCircleRotation = 360f / (Ability.GetStat("Thorns Released") + GameData.InGameAttributes.ProjectilesAdd) * i;
            var _instance = Instantiate(Prefab, GameData.player.transform.position, Quaternion.AngleAxis(instanceCircleRotation + SPRITE_OFFSET, Vector3.forward));
            Vector2 direction = new(Mathf.Cos(instanceCircleRotation * Mathf.Deg2Rad), Mathf.Sin(instanceCircleRotation * Mathf.Deg2Rad));

            _instance.Init(damage, Ability.GetStat("Thorn Size"), direction, Speed);
        }
    }

    public override void Activate()
    {
        Damage.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        Damage.AddModifier(GameData.InGameAttributes.PlayerDamageMultModifier);
        GameData.player.DamageableEntity.OnDamageTaken += OnPlayerDamageTaken;
    }
}
