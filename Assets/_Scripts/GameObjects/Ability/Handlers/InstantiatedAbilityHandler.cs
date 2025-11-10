using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedAbilityHandler : AbilityHandler
{
    private InstantiatedAbilityMono _standardPrefab;
    private InstantiatedAbilityMono _evoPrefab;

    public Stat Speed;
    public float SpawnDelay;
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
        Speed = source.speed;
        Speed.AddModifier(GameData.InGameAttributes.ProjectileSpeedMultModifier);
        SpawnDelay = source.SpawnDelay;
        damage = source.damage;
        localProjectilesAmount = source.localProjectilesAmount;
        localProjectilesAmount.AddModifier(GameData.InGameAttributes.ProjectilesAddModifier);
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
            StartCoroutine(SpawnProjectiles(_evoPrefab, SpawnDelay));
        }
        else
        {
            StartCoroutine(SpawnProjectiles(_standardPrefab, SpawnDelay));
        }
        base.Activate();
    }

    protected IEnumerator SpawnProjectiles(InstantiatedAbilityMono prefab, float delay = 0f)
    {
        for (int i = 0; i < localProjectilesAmount; i++)
        {
            var instance = Instantiate(prefab, GameData.player.transform.position, Quaternion.identity);
            instance.Init(this);
            Instances.Add(instance);
            yield return new WaitForSeconds(delay);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _standardPrefab.Upgrade(this);
    }
}
