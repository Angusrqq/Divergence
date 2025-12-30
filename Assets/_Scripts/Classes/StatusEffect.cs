using System;
using UnityEngine;

public enum StatusType
{
    Positive,
    Negative
}

public class StatusEffect
{
    protected string _name;
    protected int _timesApplied;
    protected int _ticks;
    protected Enemy _target;
    protected MonoBehaviour _sender;
    protected float _delayBetweenTicks = 0f;
    protected Action<StatusEffect> _tickMethod;
    protected double _lastTickTime;

    public StatusType Type { get; protected set; }
    public int TimesApplied { get => _timesApplied; set => _timesApplied = value; }
    public string Name { get => _name; private set => _name = value; }

    /// <param name="tickMethod"> The method that will be used every tick in <see cref="StatusHolder"/>. if none passed, it will use the <see cref="TickMethod"/> of this class</param>
    public StatusEffect(
        StatusType type,
        MonoBehaviour sender,
        string name,
        Enemy target,
        Action<StatusEffect> tickMethod = null,
        int timesApplied = 1,
        float delayBetweenTicks = 0f,
        int ticks = 1
    )
    {
        Type = type;
        _name = name;
        _sender = sender;
        _target = target;

        _ticks = ticks;
        _timesApplied = timesApplied;
        _delayBetweenTicks = delayBetweenTicks;
        _tickMethod = tickMethod ?? TickMethod;
    }

    /// <summary>
    /// <c>operator +</c> combines two <see cref="StatusEffect"/> instances of the same type and name into a new instance.
    /// <para>
    /// The resulting instance has the sum of their <see cref="TimesApplied"/> and the greater number of ticks.
    /// </para>
    /// </summary>
    public static StatusEffect operator + (StatusEffect left, StatusEffect right)
    {
        if (left != right) throw new SystemException("StatusEffects with differing names and types cannot be combined");

        Action<StatusEffect> method = left._tickMethod; // == right._tickMethod ? left._tickMethod : left._tickMethod + right._tickMethod;

        return new StatusEffect(
            left.Type,
            left._sender,
            left._name,
            left._target,
            left._tickMethod,
            left.TimesApplied + right.TimesApplied,
            left._delayBetweenTicks,
            left._ticks > right._ticks ? left._ticks : right._ticks
        );
    }

    public static bool operator == (StatusEffect left, StatusEffect right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        if (ReferenceEquals(left, right)) return true;

        return left._name == right._name && left.Type == right.Type;
    }

    public static bool operator != (StatusEffect left, StatusEffect right)
    {
        if (left is null && right is null) return false;
        if (left is null || right is null) return true;
        if (ReferenceEquals(left, right)) return false;

        return left._name != right._name && left.Type != right.Type;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as StatusEffect);
    }

    public bool Equals(StatusEffect other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;

        return _name == other._name && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name, Type);
    }

    /// <summary>
    /// <c>Tick</c> method handles the ticking behavior of the status effect, applying the tick method at specified intervals and managing the duration of the effect.
    /// </summary>
    public virtual void Tick()
    {
        if (Time.timeAsDouble - _lastTickTime >= _delayBetweenTicks && _timesApplied > 0)
        {
            _tickMethod(this);
            _lastTickTime = Time.timeAsDouble;
            _ticks--;
        }

        if (_ticks <= 0)
        {
            _target.Statuses.RemoveStatusEffect(this);
            OnRemove();
        }
    }

    protected virtual void TickMethod(StatusEffect source)
    {
        throw new NotImplementedException();
    }

    public virtual void OnApply() { }
    protected virtual void OnRemove() { }
}

public class NegativeStatusEffect : StatusEffect
{
    protected float _damage;

    public NegativeStatusEffect(
        MonoBehaviour sender,
        string name,
        Enemy target,
        Action<StatusEffect> tickMethod = null,
        int timesApplied = 1,
        float delayBetweenTicks = 0f,
        int ticks = 1,
        float damage = 0f
    )
    : base(StatusType.Negative, sender, name, target, tickMethod, timesApplied, delayBetweenTicks, ticks)
    {
        _damage = damage;
    }

    protected override void TickMethod(StatusEffect source)
    {
        _target.TakeDamage(
            source: _sender.gameObject,
            amount: _damage * source.TimesApplied,
            type: GetType(),
            flashColor: Color.red,
            useParticles: false
        );
    }
}

public class PositiveStatusEffect : StatusEffect
{
    public PositiveStatusEffect(
        MonoBehaviour sender,
        string name,
        Enemy target,
        Action<StatusEffect> tickMethod = null,
        int timesApplied = 1,
        float delayBetweenTicks = 0f
    )
    : base(StatusType.Positive, sender, name, target, tickMethod, timesApplied, delayBetweenTicks) { }
}

public class Burn : NegativeStatusEffect
{
    public Burn(
        MonoBehaviour sender,
        Enemy target,
        Action<StatusEffect> tickMethod = null,
        int timesApplied = 1,
        float delayBetweenTicks = 1f,
        int ticks = 5,
        float damage = 0f
    )
    : base(sender, "burn", target, tickMethod, timesApplied, delayBetweenTicks, ticks, damage) { }
}

public class Acid : NegativeStatusEffect
{
    private StatModifier _slowModifier;

    public Acid(
        MonoBehaviour sender,
        Enemy target,
        Action<StatusEffect> tickMethod = null,
        int timesApplied = 1,
        float delayBetweenTicks = 1f,
        int ticks = 5,
        float damage = 0f,
        float percentSlow = 0f
    )
    : base(sender, "acid", target, tickMethod, timesApplied, delayBetweenTicks, ticks, damage)
    {
        _slowModifier = new(percentSlow, StatModifierType.Percent, this);
    }

    protected override void TickMethod(StatusEffect source)
    {
        _target.TakeDamage(
            source: _sender.gameObject,
            amount: _damage * source.TimesApplied,
            type: GetType(),
            flashColor: Color.green,
            useParticles: false
        );
    }

    public override void OnApply()
    {
        _target.moveSpeed.AddModifier(_slowModifier);
    }

    protected override void OnRemove()
    {
        _target.moveSpeed.RemoveModifier(_slowModifier);
    }
}
