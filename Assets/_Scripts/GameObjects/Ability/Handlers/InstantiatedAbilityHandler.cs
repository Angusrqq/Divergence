using System.Collections.Generic;
using UnityEngine;

public class InstantiatedAbilityHandler : AbilityHandler
{
    private InstantiatedAbilityMono _standardPrefab;
    private InstantiatedAbilityMono _evoPrefab;

    public float speed;
    public float damage;
    public Stat localProjectilesAmount = 1; // How many projectiles are fired in a single burst
    public Character nativeUser; // Character for which this ability is considered native; toggles evolved state when active.
    public List<InstantiatedAbilityMono> Instances { get; private set; }

    void Start()
    {
        if (GameData.currentCharacter == nativeUser)
        {
            IsEvolved = true;
        }
    }

    protected override void AfterInit()
    {
        InstantiatedAbilityScriptable source = _source as InstantiatedAbilityScriptable;
        Instances = new List<InstantiatedAbilityMono>();
        _standardPrefab = source.StandardPrefab;
        _evoPrefab = source.EvoPrefab;
        speed = source.speed;
        damage = source.damage;
        localProjectilesAmount = source.localProjectilesAmount;
        localProjectilesAmount.AddModifier(Attributes.ProjectilesAddModifier);
        nativeUser = source.nativeUser;
        base.AfterInit();
    }

    /// <summary>
    /// Spawns one or more instances of the configured prefab at the player's position.
    /// </summary>
    /// <remarks>
    /// For each projectile to spawn, this method instantiates the standard or evolved prefab,
    /// calls <see cref="InstantiatedAbilityMono.Init(InstantiatedAbilityScriptable)"/> to provide context
    /// (including speed, damage, and lifetime), and then pushes the instance into <see cref="Instances"/>.
    /// </remarks>
    public override void Activate()
    {
        if (IsEvolved && _evoPrefab != null)
        {
            for (int i = 0; i < localProjectilesAmount; i++)
            {
                var instance = Instantiate(_evoPrefab, GameData.player.transform.position, Quaternion.identity);
                instance.Init(this);
                Instances.Add(instance);
            }
        }
        else
        {
            for (int i = 0; i < localProjectilesAmount; i++)
            {
                var instance = Instantiate(_standardPrefab, GameData.player.transform.position, Quaternion.identity);
                instance.Init(this);
                Instances.Add(instance);
            }
        }
        base.Activate();
    }
}
