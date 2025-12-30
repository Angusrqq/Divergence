using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum AbilityState
{
    ready,
    active,
    cooldown
}

/// <summary>
/// <c>AbilityHolder</c> is a class that holds and handles the abilities of a character.
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

    public List<BaseAbilityHandler> GetAllAbilities() => Abilities.Concat(Passives).ToList();
    public List<BaseAbilityHandler> GetActiveAbilitiesList() => Abilities;
    public List<BaseAbilityHandler> GetPassiveAbilitiesList() => Passives;

    // TODO: Refactor the whole ability system, currently it's a mess (im meaning not just changing the naming, but make the code more concise/readable).
    // Example: Currently AddAbility adds the Instantiated version. Either rename it or make it use the base Ability class or split the functions(if splitting, where is DRY???)
    public void AddAbility(Ability ability)
    {
        if (_abilityNames.Contains(ability.Name))
        {
            BaseAbilityHandler temp = GetAbilityByName(ability.Name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }

            GameData.player.PlayerAbilityIconDisplay.UpdateActiveAbilitiesIcons(Abilities);
            return;
        }
        
        BaseAbilityHandler abilityInstance = CreateHandler(ability.Type, ability.Name);
        abilityInstance.Init(ability);
        Abilities.Add(abilityInstance);
        _abilityNames.Add(abilityInstance.Name);

        GameData.player.PlayerAbilityIconDisplay.UpdateActiveAbilitiesIcons(Abilities);

        Debug.Log($"Active ability added: {abilityInstance.Name}");
    }

    public void AddPassive(PassiveAbility passive)
    {
        if (_passiveNames.Contains(passive.Name))
        {
            BaseAbilityHandler temp = GetPassiveByName(passive.Name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }

            GameData.player.PlayerAbilityIconDisplay.UpdatePassiveAbilitiesIcons(Passives);
            return;
        }

        BaseAbilityHandler passiveInstance = CreateHandler(passive.Type, passive.Name);
        passiveInstance.Init(passive);

        Passives.Add(passiveInstance);
        _passiveNames.Add(passiveInstance.Name);

        var logic = Instantiate(passive.MonoLogic, passiveInstance.transform);
        var tempHandler = passiveInstance as PassiveAbilityHandler;
        tempHandler.SetMonoLogic(logic);

        passiveInstance.Activate();
        GameData.player.PlayerAbilityIconDisplay.UpdatePassiveAbilitiesIcons(Passives);

        Debug.Log($"Passive ability added: {passiveInstance.Name}");
    }

    public BaseAbilityHandler GetPassiveByName(string name)
    {
        if (_passiveNames.IndexOf(name) == -1) return null;

        return Passives[_passiveNames.IndexOf(name)];
    }

    public BaseAbilityHandler GetAbilityByName(string name)
    {
        if (_abilityNames.IndexOf(name) == -1) return null;

        return Abilities[_abilityNames.IndexOf(name)];
    }

    public BaseAbilityHandler CreateHandler(HandlerType type, string abilityName = "Ability")
    {
        GameObject container = new(abilityName + "Handler");
        container.transform.parent = ParentHolder.transform;

        BaseAbilityHandler handler = type switch
        {
            HandlerType.BaseAbility => container.AddComponent<BaseAbilityHandler>(),
            HandlerType.Ability => container.AddComponent<AbilityHandler>(),
            HandlerType.InstantiatedAbility => container.AddComponent<InstantiatedAbilityHandler>(),
            HandlerType.Passive => container.AddComponent<PassiveAbilityHandler>(),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

        AudioSource source = handler.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = AudioManager.instance.Mixer.FindMatchingGroups("SFX")[0];

        return handler;
    }

    void OnDestroy()
    {
        OnEnemyHit = null;
        OnProjectileFired = null;
        OnProjectileHit = null;
        OnAbilityActivated = null;
    }

    public void TriggerOnEnemyHit(Type abilityType, Enemy target, float damage, InstantiatedAbilityMono projectile = null)
    {
        OnEnemyHit?.Invoke(abilityType, target, damage, projectile);
        GameData.InGameAttributes.DamageDealt += damage;
    }

    public BaseAbilityHandler GetHandlerForAbility(BaseAbilityScriptable ability)
    {
        if (ability.Type == HandlerType.Passive)
        {
            return GameData.player.AbilityHolder.GetPassiveByName(ability.Name);
        }

        if (ability.Type == HandlerType.InstantiatedAbility)
        {
            return GameData.player.AbilityHolder.GetAbilityByName(ability.Name);
        }

        return null;
    }

    public void TriggerOnProjectileFired(Type abilityType, InstantiatedAbilityMono projectile)
    {
        OnProjectileFired?.Invoke(abilityType, projectile);
    }

    public void TriggerOnProjectileHit(Type abilityType, Vector2 position)
    {
        OnProjectileHit?.Invoke(abilityType, position);
    }

    public void TriggerOnAbilityActivated(Type abilityType, InstantiatedAbilityHandler ability, InstantiatedAbilityMono prefab)
    {
        OnAbilityActivated?.Invoke(abilityType, ability, prefab);
    }
}
