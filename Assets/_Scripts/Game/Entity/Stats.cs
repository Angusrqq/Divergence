using System;
using System.Collections.Generic;

public enum StatModifierType
{
    Flat,
    Additive,
    Multiplicative
}

[Serializable]
public class Stat
{
    public float BaseValue;
    public float BaseModifier;
    public float BaseMultiplier;

    private bool _recalculationNeeded;
    private float _value;
    private readonly List<StatModifier> _flatModifiers;
    private readonly List<StatModifier> _additiveModifiers;
    private readonly List<StatModifier> _multiplicativeModifiers;
    public int TotalModifiers => _flatModifiers.Count + _additiveModifiers.Count + _multiplicativeModifiers.Count;

    public Stat(float BaseValue, float BaseModifier = 1, float BaseMultiplier = 1)
    {
        this.BaseValue = BaseValue;
        this.BaseModifier = BaseModifier;
        this.BaseMultiplier = BaseMultiplier;
        _flatModifiers = new List<StatModifier>();
        _additiveModifiers = new List<StatModifier>();
        _multiplicativeModifiers = new List<StatModifier>();
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        _recalculationNeeded = true;
        switch (modifier.type)
        {
            case StatModifierType.Flat:
                _flatModifiers.Add(modifier);
                break;
            case StatModifierType.Additive:
                _additiveModifiers.Add(modifier);
                break;
            case StatModifierType.Multiplicative:
                _multiplicativeModifiers.Add(modifier);
                break;
        }
    }
    public virtual bool RemoveModifier(StatModifier modifier)
    {
        _recalculationNeeded = true;
        switch (modifier.type)
        {
            case StatModifierType.Flat:
                return _flatModifiers.Remove(modifier);
            case StatModifierType.Additive:
                return _additiveModifiers.Remove(modifier);
            case StatModifierType.Multiplicative:
                return _multiplicativeModifiers.Remove(modifier);
            default:
                return false;
        }
    }

    public virtual float Value
    {
        get
        {
            if (TotalModifiers == 0) return BaseValue;
            if (_recalculationNeeded)
            {
                _value = Calculate();
                _recalculationNeeded = false;
            }
            return _value;
        }
        set => BaseValue = value;
    }

    public virtual float Calculate()
    {
        float flatValue = BaseValue;
        foreach (StatModifier modifier in _flatModifiers)
        {
            flatValue += modifier.Value;
        }
        float additiveValue = BaseModifier;
        foreach (StatModifier modifier in _additiveModifiers)
        {
            additiveValue += modifier.Value;
        }
        float multiplicativeValue = BaseMultiplier;
        foreach (StatModifier modifier in _multiplicativeModifiers)
        {
            multiplicativeValue += modifier.Value;
        }
        return flatValue * additiveValue * multiplicativeValue;
    }

    public static implicit operator float(Stat stat) => stat.Value;
    public static implicit operator Stat(float value) => new(value);
    public static implicit operator int(Stat stat) => (int)stat.Value;
    public override string ToString() => Value.ToString();
}

// public class StatF : Stat<float>
// {
//     public StatF(float BaseValue, float BaseModifier, float BaseMultiplier) : base(BaseValue, BaseModifier, BaseMultiplier) { }
// }

// public class StatInt : Stat<int>
// {
//     public StatInt(int BaseValue, int BaseModifier, int BaseMultiplier) : base(BaseValue, BaseModifier, BaseMultiplier) { }
// }

public class StatModifier
{
    public readonly float Value;
    public readonly object Source;
    //public Func<float, float> Operation;
    public StatModifierType type;

    public StatModifier(float Value, StatModifierType type, object Source)
    {
        this.Value = Value;
        this.type = type;
        this.Source = Source;
    }
}

// public class FlatStatModifier<T> : StatModifier<T>
// {
//     public FlatStatModifier(T Value, object Source, Func<T, T> Operation) : base(Value, Source, Operation) { }
// }

// public class AdditiveStatModifier<T> : StatModifier<T>
// {
//     public AdditiveStatModifier(T Value, object Source, Func<T, T> Operation) : base(Value, Source, Operation) { }
// }

// public class MultiplicativeStatModifier<T> : StatModifier<T>
// {
//     public MultiplicativeStatModifier(T Value, object Source, Func<T, T> Operation) : base(Value, Source, Operation) { }
// }