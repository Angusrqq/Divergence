using System;
using System.Collections.Generic;

public enum StatModifierType
{
    Flat,
    Percent,
    Mult
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
    private readonly List<StatModifier> _percentModifiers;
    private readonly List<StatModifier> _multModifiers;
    public int TotalModifiers => _flatModifiers.Count + _percentModifiers.Count + _multModifiers.Count;

    public Stat(float BaseValue, float BaseModifier = 1, float BaseMultiplier = 1, List<StatModifier> flatModifiers = null,
    List<StatModifier> percentModifiers = null, List<StatModifier> multModifiers = null)
    {
        this.BaseValue = BaseValue;
        this.BaseModifier = BaseModifier;
        this.BaseMultiplier = BaseMultiplier;
        _flatModifiers = flatModifiers ?? new();
        _percentModifiers = percentModifiers ?? new();
        _multModifiers = multModifiers ?? new();
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        if (modifier.GetType() == typeof(StatModifierByStat) && ((StatModifierByStat)modifier).Stat == this) throw new ArgumentException("Cannot add modifier that contains a stat reference to itself");
        _recalculationNeeded = true;
        switch (modifier.type)
        {
            case StatModifierType.Flat:
                _flatModifiers.Add(modifier);
                break;
            case StatModifierType.Percent:
                _percentModifiers.Add(modifier);
                break;
            case StatModifierType.Mult:
                _multModifiers.Add(modifier);
                break;
        }
        modifier.OnValueChanged += OnModifierValueChanged;
    }
    public virtual bool RemoveModifier(StatModifier modifier)
    {
        _recalculationNeeded = true;
        modifier.OnValueChanged -= OnModifierValueChanged;
        return modifier.type switch
        {
            StatModifierType.Flat => _flatModifiers.Remove(modifier),
            StatModifierType.Percent => _percentModifiers.Remove(modifier),
            StatModifierType.Mult => _multModifiers.Remove(modifier),
            _ => false,
        };
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
        foreach (StatModifier modifier in _percentModifiers)
        {
            additiveValue += modifier.Value;
        }
        float multiplicativeValue = BaseMultiplier;
        foreach (StatModifier modifier in _multModifiers)
        {
            multiplicativeValue += modifier.Value;
        }
        return flatValue * additiveValue * multiplicativeValue;
    }

    public static implicit operator float(Stat stat) => stat.Value;
    public static implicit operator Stat(float value) => new(value);
    public static implicit operator int(Stat stat) => (int)stat.Value;
    public override string ToString() => Value.ToString();

    private void OnModifierValueChanged(StatModifier modifier) => _recalculationNeeded = true;
}

public class StatModifier
{
    private float _value;
    public virtual float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            ValueChanged();
        }
    }
    public event Action<StatModifier> OnValueChanged;
    public readonly object Source;
    public StatModifierType type;

    public StatModifier(float Value, StatModifierType type, object Source)
    {
        this.Value = Value;
        this.type = type;
        this.Source = Source;
    }

    protected virtual void ValueChanged() => OnValueChanged?.Invoke(this);

    public static implicit operator float(StatModifier modifier) => modifier.Value;
    public override string ToString()
    {
        return Value.ToString();
    }
}

public class StatModifierByStat: StatModifier
{
    public Stat Stat;
    public override float Value
    {
        get
        {
            return Stat;
        }
        set
        {
            Stat = value;
            ValueChanged();
        }
    }

    public StatModifierByStat(ref Stat Stat, StatModifierType type, object Source) : base(Stat, type, Source)
    {
        this.Stat = Stat;
    }
}