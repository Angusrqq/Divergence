using System;
using System.Collections;
using UnityEngine;

public class AnsweredPrayer : PassiveAbilityMono
{
    public PrayerShockwave FieldPrefab;
    private Stat _thresholdMult = 0.15f;
    private bool _isActive = false;
    private Coroutine _activeCoroutine = null;

    void OnHealthChanged(UnityEngine.Object source, float amount, Type type = null)
    {
        if (GameData.player.DamageableEntity.Health <= GameData.player.DamageableEntity.MaxHealth * _thresholdMult && !_isActive)
        {
            _isActive = true;
            ActivateEffect();
        }
        else if (GameData.player.DamageableEntity.Health > GameData.player.DamageableEntity.MaxHealth * _thresholdMult && _isActive)
        {
            _isActive = false;
            DeactivateEffect();
        }
    }

    private void ActivateEffect()
    {
        _activeCoroutine = StartCoroutine(Effect());
    }

    private void DeactivateEffect()
    {
        if (_activeCoroutine == null) return;
        StopCoroutine(_activeCoroutine);
    }

    private IEnumerator Effect()
    {
        AudioManager.instance.PlaySFX(AudioClips[0]);
        while (true)
        {
            if (GameData.MidValue < Ability.GetStat("Wave spawn chance"))
            {
                Instantiate(FieldPrefab, GameData.player.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(Ability.GetStat("Cooldown"));
        }
    }

    public override void Activate()
    {
        Ability.GetStat("Wave spawn chance").AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        Ability.GetStat("Cooldown").AddModifier(GameData.InGameAttributes.CooldownReductionMultModifier);
        GameData.player.DamageableEntity.OnDamageTaken += OnHealthChanged;
    }

    // public override void Upgrade()
    // {
    //     _chance.Value *= 2f;
    //     _cooldown.Value -= 1f;
    // }
}
