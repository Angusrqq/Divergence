using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability that spawns runtime instances of <see cref="InstantiatedAbilityMono"/> when activated.
/// </summary>
/// <remarks>
/// On <see cref="Activate"/>, this asset instantiates either the standard or evolved prefab at the
/// player's position, calls <see cref="InstantiatedAbilityMono.Init(InstantiatedAbilityScriptable)"/>, and
/// registers the instance in <see cref="Instances"/>. Per-tick movement, lifetime countdown, collision
/// damage, and self-unregistration are handled by the spawned <see cref="InstantiatedAbilityMono"/>.
/// </remarks>
[CreateAssetMenu(fileName = "New InstantiatedAbility", menuName = "Abilities/InstantiatedAbility")]
public class InstantiatedAbilityScriptable : Ability
{
    [SerializeField] private InstantiatedAbilityMono _standardPrefab;
    [SerializeField] private InstantiatedAbilityMono _evoPrefab;

    public float speed;
    public float damage;
    public int localProjectilesAmount = 1; // How many projectiles are fired in a single burst
    public Character nativeUser; // Character for which this ability is considered native; toggles evolved state when active.
    public List<InstantiatedAbilityMono> Instances { get; private set; }

    void Awake()
    {
        if (GameData.currentCharacter == nativeUser)
        {
            IsEvolved = true;
        }
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
            for(int i = 0; i < localProjectilesAmount + Attributes.ProjectilesAdd; i++)
            {
                var instance = Instantiate(_evoPrefab, GameData.player.transform.position, Quaternion.identity);
                instance.Init(this);
                Instances.Add(instance);
            }
        }
        else
        {
            for(int i = 0; i < localProjectilesAmount + Attributes.ProjectilesAdd; i++)
            {
                var instance = Instantiate(_standardPrefab, GameData.player.transform.position, Quaternion.identity);
                instance.Init(this);
                Instances.Add(instance);
            }
        }
        base.Activate();
    }

    /// <summary>
    /// <para>
    /// This is the method that is called when the ability is deactivated
    /// </para>
    /// destroys the instance of the prefab
    /// </summary>
    // public override void StartCooldown()
    // {
    //     if (_instance) { Destroy(_instance.gameObject); _instance = null; }
    //     base.StartCooldown();
    // }
}
