using UnityEngine;

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

    public override BaseAbilityMono Behaviour => IsEvolved ? EvoPrefab : StandardPrefab;
    public override HandlerType Type => HandlerType.InstantiatedAbility;
}
