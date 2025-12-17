using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatusHolder class represents a container for managing multiple <see cref="StatusEffect"/> instances applied to an <see cref="Enemy"/>.
/// <para>
/// It includes methods for applying and removing status effects, as well as running the status effect ticks.
/// </para>
/// </summary>
public class StatusHolder
{
    private List<StatusEffect> _effectBuffer = new();
    public List<StatusEffect> EffectBuffer => _effectBuffer;
    private List<StatusEffect> _effectsToDump = new();

    /// <summary>
    /// <c>RunTicks</c> method iterates through the list of status effects and calls their <see cref="StatusEffect.Tick"/> method.
    /// <para>
    /// It also handles the removal of status effects that have reached their maximum duration.
    /// </para>
    /// </summary>
    public void RunTicks()
    {
        if (_effectBuffer.Count == 0) return;
        foreach (StatusEffect effect in _effectBuffer)
        {
            effect.Tick();
        }
        foreach (StatusEffect effect in _effectsToDump)
        {
            _effectBuffer.Remove(effect);
        }
        _effectsToDump.Clear();
    }

    /// <summary>
    /// <c>ApplyEffect</c> method applies a passed <see cref="StatusEffect"/> to the <see cref="StatusHolder"/>.
    /// </summary>
    /// <param name="effect">the <see cref="StatusEffect"/> to be applied</param>
    public void ApplyEffect(StatusEffect effect)
    {
        StatusEffect foundEffect = _effectBuffer.Find(e => e.Name == effect.Name);
        if (foundEffect != null)
        {
            _effectBuffer.Remove(foundEffect);
            _effectBuffer.Add(foundEffect + effect);
        }
        else
        {
            _effectBuffer.Add(effect);
            effect.OnApply();
        }
    }

    /// <summary>
    /// <c>RemoveStatusEffect</c> method removes a passed <see cref="StatusEffect"/> from the <see cref="StatusHolder"/>.
    /// </summary>
    /// <param name="effect">the <see cref="StatusEffect"/> to be removed</param>
    public void RemoveStatusEffect(StatusEffect effect)
    {
        if (_effectBuffer.Contains(effect) == false) return;
        _effectsToDump.Add(effect);
    }
}
