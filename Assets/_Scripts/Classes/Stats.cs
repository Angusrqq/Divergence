using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of the stat modifier: Flat, Percent, or Multiplier.
/// <para>
/// <c>Flat</c> Direct addition to the base value.
/// </para>
/// <para>
/// <c>Percent</c> Percentage-based addition to the base value.
/// </para>
/// <c>Mult</c> Multiplicative factor applied to the base value.
/// </summary>
public enum StatModifierType
{
    /// <summary>
    /// Flat modifier type adds a fixed value to the base stat.
    /// </summary>
    Flat,
    /// <summary>
    /// Percent modifier type adds a percentage-based value to the base stat.
    /// </summary>
    Percent,
    /// <summary>
    /// Mult modifier type multiplies the base stat by a factor.
    /// </summary>
    Mult
}
/// <summary>
/// Stat class represents a character or object attribute that can be modified by various modifiers.
/// <para>
/// It uses three lists of <see cref="StatModifier"/> for each <see cref="StatModifierType"/> .
/// </para>
/// Order of operations: <see cref="StatModifierType.Flat"/> -> <see cref="StatModifierType.Percent"/> -> <see cref="StatModifierType.Mult"/>
/// </summary>
[Serializable]
public class Stat
{
    public float BaseValue;
    public float BaseModifier;
    public float BaseMultiplier;
    public Action OnRecalculation;

    private bool _recalculationNeeded;
    private float _value;
    private readonly List<StatModifier> _flatModifiers;
    private readonly List<StatModifier> _percentModifiers;
    private readonly List<StatModifier> _multModifiers;

    public int TotalModifiers => _flatModifiers.Count + _percentModifiers.Count + _multModifiers.Count;
    public static implicit operator float(Stat stat) => stat.Value;
    public static implicit operator Stat(float value) => new(value);
    //public static implicit operator int(Stat stat) => (int)stat.Value;

    /// <summary>
    /// <c>Stat</c> constructor initializes a new instance of the Stat class with the specified base value, modifier, multiplier, and optional lists of modifiers.
    /// </summary>
    /// <param name="BaseValue">Base float value of the stat</param>
    /// <param name="BaseModifier">Base float percentage of the stat</param>
    /// <param name="BaseMultiplier">Base float multiplier of the stat</param>
    /// <param name="flatModifiers">Optional list of <see cref="StatModifier"/>s with <see cref="StatModifierType.Flat"/> type </param>
    /// <param name="percentModifiers">Optional list of <see cref="StatModifier"/>s with <see cref="StatModifierType.Percent"/> type </param>
    /// <param name="multModifiers">Optional list of <see cref="StatModifier"/>s with <see cref="StatModifierType.Mult"/> type </param>
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

    /// <summary>
    /// <c>AddModifier</c> method adds a <see cref="StatModifier"/> to the appropriate list based on its type.
    /// </summary>
    /// <param name="modifier"><see cref="StatModifier"/> to add</param>
    /// <exception cref="ArgumentException">thrown to prevent stack overflow (i think)</exception>
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

    /// <summary>
    /// Removes a <see cref="StatModifier"/> from the appropriate list based on its type.
    /// </summary>
    /// <param name="modifier"><see cref="StatModifier"/> to remove</param>
    /// <returns>result of <see cref="List{StatModifier}.Remove(StatModifier)"/> if the type is recognized, else false</returns>
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

    /// <summary>
    /// <c>Value</c> property gets or sets the calculated value of the stat, recalculating it if necessary.
    /// </summary>
    public virtual float Value
    {
        get
        {
            if (TotalModifiers == 0) return BaseValue;
            if (_recalculationNeeded)
            {
                _value = Calculate();
                OnRecalculation?.Invoke();
                _recalculationNeeded = false;
            }
            return _value;
        }
        set
        {
            BaseValue = value;
            _recalculationNeeded = true;
        }
    }

    /// <summary>
    /// <c>Calculate</c> method computes the final value of the stat by applying all modifiers in the correct order.
    /// </summary>
    /// <returns>result of the calculation</returns>
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

    public override string ToString() => Value.ToString();

    private void OnModifierValueChanged(StatModifier modifier) => _recalculationNeeded = true;
}

/// <summary>
/// StatModifier class represents a modification to a Stat, with a float value, <see cref="StatModifierType"/> type, and object source.
/// </summary>
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

    /// <summary>
    /// <c>StatModifier</c> constructor initializes a new instance of the StatModifier class with the specified value, type, and source.
    /// </summary>
    /// <param name="Value">the value to be used in <see cref="Stat.Calculate"/></param>
    /// <param name="type"><see cref="StatModifierType"/> to be associated with this modifier</param>
    /// <param name="Source">Source that applied the modifier</param>
    public StatModifier(float Value, StatModifierType type, object Source)
    {
        _value = Value;
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

/// <summary>
/// StatModifierByStat class represents a modification to a <see cref="Stat."/> Where the value is derived from another Stat.
/// <para>
/// It inherits from <see cref="StatModifier"/> and overrides the Value property to get and set the value of the referenced Stat.
/// </para>
/// If the referenced Stat is the same as the Stat being modified, an <see cref="ArgumentException"/> is thrown to prevent infinite recursion.
/// </summary>
public class StatModifierByStat : StatModifier
{
    public Stat Stat;
    private readonly bool _subtractOne;
    public override float Value
    {
        get
        {
            return _subtractOne ? Stat - 1 : Stat;
        }
        set
        {
            Stat.Value = value;
        }
    }

    private void OnStatValueChanged() => ValueChanged();

    /// <summary>
    /// <c>StatModifierByStat</c> constructor initializes a new instance of the StatModifierByStat class with the specified Stat, type, and source.
    /// </summary>
    /// <param name="Stat">reference to the <see cref="Stat."/>, value of which will be used in <see cref="Stat.Calculate"/></param>
    /// <param name="type"><see cref="StatModifierType"/> to be associated with this modifier</param>
    /// <param name="Source">Source that applied the modifier</param>
    public StatModifierByStat(ref Stat Stat, StatModifierType type, object Source, bool subtractOne = false) : base(Stat, type, Source)
    {
        this.Stat = Stat;
        Stat.OnRecalculation += OnStatValueChanged;
        _subtractOne = subtractOne;
    }
}
