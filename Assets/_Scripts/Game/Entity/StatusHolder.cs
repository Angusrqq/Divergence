using System.Collections.Generic;
using UnityEngine;

public class StatusHolder
{
    private List<StatusEffect> _effectBuffer = new();
    public List<StatusEffect> EffectBuffer => _effectBuffer;

    public void RunTicks()
    {
        if (_effectBuffer.Count == 0) return;
        foreach (StatusEffect effect in _effectBuffer)
        {
            effect.Tick();
        }
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
        }
    }
}
