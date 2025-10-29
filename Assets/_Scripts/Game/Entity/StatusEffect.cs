using System;
using System.Data;
using UnityEngine;

public enum StatusType
{
    Positive,
    Negative
}

public class StatusEffect
{
    public StatusType Type { get; protected set; }
    protected string _name;
    protected int _timesApplied;
    protected int _ticks;
    public int TimesApplied { get => _timesApplied; set => _timesApplied = value; }
    public string Name { get => _name; private set => _name = value; }
    protected Enemy _target;
    protected MonoBehaviour _sender;
    protected float _delayBetweenTicks = 0f;
    protected Action _tickMethod;
    protected double _lastTickTime;

    public StatusEffect(StatusType type, MonoBehaviour sender, string name, Enemy target, Action tickMethod = null,
        int timesApplied = 1, float delayBetweenTicks = 0f, int ticks = 1)
    {
        Type = type;
        _name = name;
        _sender = sender;
        _ticks = ticks;
        _target = target;
        _timesApplied = timesApplied;
        _delayBetweenTicks = delayBetweenTicks;
        _tickMethod = tickMethod ?? TickMethod;
    }
    public static StatusEffect operator +(StatusEffect left, StatusEffect right)
    {
        if (left != right) throw new SystemException("StatusEffects with differing names and types cannot be combined");
        Action method = left._tickMethod == right._tickMethod ? left._tickMethod : left._tickMethod + right._tickMethod;
        if(left._tickMethod != right._tickMethod)
        {
            Debug.LogWarning($"[StatusEffect operator +] {left} tickMethod differs from {right}, they will be combined");
        }
        return new StatusEffect(
            left.Type,
            left._sender,
            left._name,
            left._target,
            method,
            left.TimesApplied + right.TimesApplied,
            left._delayBetweenTicks,
            left._ticks > right._ticks ? left._ticks : right._ticks);
    }

    public static bool operator ==(StatusEffect left, StatusEffect right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        if (ReferenceEquals(left, right)) return true;
        return left._name == right._name && left.Type == right.Type;
    }

    public static bool operator !=(StatusEffect left, StatusEffect right)
    {
        if (left is null && right is null) return false;
        if (left is null || right is null) return true;
        if (ReferenceEquals(left, right)) return false;
        return left._name != right._name && left.Type != right.Type;
    }
    //--------generated stuff i dont understand
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
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + (_name != null ? _name.GetHashCode() : 0);
            hash = hash * 31 + Type.GetHashCode();
            return hash;
        }
    }
    //--------end of generated stuff i dont understand
    public virtual void Tick()
    {
        if (Time.timeAsDouble - _lastTickTime >= _delayBetweenTicks && _ticks > 0)
        {
            _tickMethod();
            Debug.Log($"Status {_name} ticked");
            _lastTickTime = Time.timeAsDouble;
            _ticks--;
        }
        if(_ticks <= 0)
        {
            _target.Statuses.RemoveStatusEffect(this);
        }
    }

    protected virtual void TickMethod()
    {
        throw new NotImplementedException();
    }
}

public class NegativeStatusEffect : StatusEffect
{
    protected float _damage;

    public NegativeStatusEffect(MonoBehaviour sender, string name, Enemy target, Action tickMethod = null, int timesApplied = 1, float delayBetweenTicks = 0f, int ticks = 1, float damage = 0f)
    : base(StatusType.Negative, sender, name, target, tickMethod, timesApplied, delayBetweenTicks, ticks)
    {
        _damage = damage;
    }

    protected override void TickMethod()
    {
        _target.TakeDamage(_sender.gameObject, _damage * _timesApplied, GetType(), flashColor: Color.red, useParticles: false);
    }
}

public class PositiveStatusEffect : StatusEffect // i feel stupid
{
    public PositiveStatusEffect(MonoBehaviour sender, string name, Enemy target, Action tickMethod = null, int timesApplied = 1, float delayBetweenTicks = 0f)
    : base(StatusType.Positive, sender, name, target, tickMethod, timesApplied, delayBetweenTicks) { }
}