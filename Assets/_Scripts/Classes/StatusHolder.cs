using System.Collections.Generic;

/// <summary>
/// StatusHolder class represents a container for managing multiple <see cref="StatusEffect"/> instances applied to an <see cref="Enemy"/>.
/// </summary>
public class StatusHolder
{
    private readonly List<StatusEffect> _effectBuffer = new();
    private readonly List<StatusEffect> _effectsToDump = new();

    public List<StatusEffect> EffectBuffer => _effectBuffer;

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
            _effectBuffer.Remove(foundEffect);
            _effectBuffer.Add(foundEffect + effect);
        }
        else
        {
            _effectBuffer.Add(effect);
            effect.OnApply();
        }
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        if (_effectBuffer.Contains(effect) == false) return;

        _effectsToDump.Add(effect);
    }
}
