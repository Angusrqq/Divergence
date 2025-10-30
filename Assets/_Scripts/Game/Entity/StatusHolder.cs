using System.Collections.Generic;
using UnityEngine;

public class StatusHolder
{
    private List<StatusEffect> _effectBuffer = new();
    public List<StatusEffect> EffectBuffer => _effectBuffer;
    private List<StatusEffect> _effectsToDump = new();

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

    public void ApplyEffect(StatusEffect effect)
    {
        StatusEffect foundEffect = _effectBuffer.Find(e => e.Name == effect.Name);
        if (foundEffect != null)
        {
            foundEffect += effect;
        }
        else
        {
            _effectBuffer.Add(effect);
            effect.OnApply();
        }
    }
    
    public void RemoveStatusEffect(StatusEffect effect)
    {
        if(_effectBuffer.Contains(effect) == false) return;
        _effectsToDump.Add(effect);
    }
}
