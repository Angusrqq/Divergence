using System;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
    ready,
    active,
    cooldown
}

/// <summary>
/// <para>
/// <c>AbilityHolder</c> is a class that holds and handles the abilities of a character.
/// </para>
/// Can add abilities and passives, and handle their upgrades.
/// </summary>
public class AbilityHolder : MonoBehaviour
{
    public List<BaseAbilityHandler> Abilities;
    public List<BaseAbilityHandler> Passives;
    public GameObject ParentHolder;

    public event Action<Type, Enemy, float, InstantiatedAbilityMono> OnEnemyHit;
    public event Action<Type, InstantiatedAbilityMono> OnProjectileFired;
    public event Action<Type, Vector2> OnProjectileHit;
    public event Action<Type, InstantiatedAbilityHandler, InstantiatedAbilityMono> OnAbilityActivated;

    private readonly List<string> _abilityNames = new();
    private readonly List<string> _passiveNames = new();

    public List<string> AbilityNames => _abilityNames;
    public List<string> PassiveNames => _passiveNames;

    void Update()
    {
        foreach (BaseAbilityHandler a in Abilities)
        {
            a.UpdateAbility();
        }
    }

    public List<BaseAbilityHandler> GetActiveAbilitiesList() => Abilities;
    public List<BaseAbilityHandler> GetPassiveAbilitiesList() => Passives;

    //TODO: refactor the whole ability system, currently it's a mess (im meaning not just changing the naming, but make the code more concise/readable).
    //Example: currently AddAbility adds the Instantiated version. Either rename it or make it use the base Ability class or split the functions(if splitting, where is DRY???)
    public void AddAbility(Ability ability)
    {
        if (_abilityNames.Contains(ability.Name))
        {
            BaseAbilityHandler temp = GetAbilityByName(ability.Name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }
            return;
        }
        
        BaseAbilityHandler abilityInstance = CreateHandler(ability.Type, ability.Name);
        abilityInstance.Init(ability);
        Abilities.Add(abilityInstance);
        _abilityNames.Add(abilityInstance.Name);
        Debug.Log("Active ability added: " + abilityInstance.Name);

        GameData.player.PlayerAbilityIconDisplay.UpdateActiveAbilitiesIcons(Abilities);
    }

    /// <summary>
    /// <para>
    /// Acts the same as <c>AddAbility</c>, but for <c>PassiveAbility</c>.
    /// </para>
    /// <para>
    /// Adds passed <c>PassiveAbility</c> to the <c>Passives</c> List if <c>_passiveNames</c> does not contain the name of added passive.
    /// And calls <c>PassiveAbility.Activate</c> on the added passive.
    /// </para> else, if the passive is already present and its level is less than its max level, upgrades the passive.
    /// </summary>
    /// <param name="passive"></param>
    public void AddPassive(PassiveAbility passive)
    {
        Debug.Log("Adding passive: " + passive.Name);
        if (_passiveNames.Contains(passive.Name))
        {
            Debug.Log("Passive already exists, upgrading if possible: " + passive.Name);
            BaseAbilityHandler temp = GetPassiveByName(passive.Name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }
            return;
        }
        Debug.Log("Passive does not exist, creating new: " + passive.Name);
        BaseAbilityHandler passiveInstance = CreateHandler(passive.Type, passive.Name);
        passiveInstance.Init(passive);
        Passives.Add(passiveInstance);
        _passiveNames.Add(passiveInstance.Name);
        if(passive.PassiveType == PassiveAbilityType.Updated)
        {
            var logic = Instantiate(passive.MonoLogic, passiveInstance.transform);
            var temp = passiveInstance as PassiveAbilityHandler;
            temp.SetMonoLogic(logic);
        }
        passiveInstance.Activate();
        Debug.Log("Passive added: " + passiveInstance.Name);

        GameData.player.PlayerAbilityIconDisplay.UpdatePassiveAbilitiesIcons(Passives);
    }

    /// <summary>
    /// <para>
    /// Returns the <c>PassiveAbility</c> from the <c>Passives</c> List with the passed name.
    /// </para>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BaseAbilityHandler GetPassiveByName(string name)
    {
        if(_passiveNames.IndexOf(name) == -1) return null;
        return Passives[_passiveNames.IndexOf(name)];
    }

    /// <summary>
    /// <para>
    /// Returns the <c>Ability</c> from the <c>Abilities</c> List with the passed name.
    /// </para>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BaseAbilityHandler GetAbilityByName(string name)
    {
        if (_abilityNames.IndexOf(name) == -1) return null;
        return Abilities[_abilityNames.IndexOf(name)];
    }

    public BaseAbilityHandler CreateHandler(HandlerType type, string abilityName = "Ability")
    {
        GameObject container = new(abilityName + "Handler");
        container.transform.parent = ParentHolder.transform;
        return type switch
        {
            HandlerType.BaseAbility => container.AddComponent<BaseAbilityHandler>(),
            HandlerType.Ability => container.AddComponent<AbilityHandler>(),
            HandlerType.InstantiatedAbility => container.AddComponent<InstantiatedAbilityHandler>(),
            HandlerType.Passive => container.AddComponent<PassiveAbilityHandler>(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    void OnDestroy()
    {
        OnEnemyHit = null;
        OnProjectileFired = null;
        OnProjectileHit = null;
        OnAbilityActivated = null;
    }

    public void TriggerOnEnemyHit(Type abilityType, Enemy target, float damage, InstantiatedAbilityMono projectile = null) => OnEnemyHit?.Invoke(abilityType, target, damage, projectile);
    public void TriggerOnProjectileFired(Type abilityType, InstantiatedAbilityMono projectile) => OnProjectileFired?.Invoke(abilityType, projectile);
    public void TriggerOnProjectileHit(Type abilityType, Vector2 position) => OnProjectileHit?.Invoke(abilityType, position);
    public void TriggerOnAbilityActivated(Type abilityType, InstantiatedAbilityHandler ability, InstantiatedAbilityMono prefab) => OnAbilityActivated?.Invoke(abilityType, ability, prefab);
}
