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
    [Header("Projectile info")]
    public InstantiatedAbilityMono StandardPrefab;
    public InstantiatedAbilityMono EvoPrefab;

    public float speed;
    public float SpawnDelay = 0f;
    public float damage;
    public int localProjectilesAmount = 1; // How many projectiles are fired in a single burst
    public Character nativeUser; // Character for which this ability is considered native; toggles evolved state when active.

    public override HandlerType Type => HandlerType.InstantiatedAbility;
}
