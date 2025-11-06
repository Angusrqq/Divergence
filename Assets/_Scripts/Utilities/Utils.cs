using System;

public static class Utils
{
    public static Type GetHandlerTypeByEnum(HandlerType type)
    {
        return type switch
        {
            HandlerType.BaseAbility => typeof(BaseAbilityHandler),
            HandlerType.Ability => typeof(AbilityHandler),
            HandlerType.InstantiatedAbility => typeof(InstantiatedAbilityHandler),
            HandlerType.Passive => typeof(BaseAbilityHandler),// funny low iq shit
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
