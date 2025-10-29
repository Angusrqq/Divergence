using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Example use of the ability system
/// </para>
/// Here we just instantiate the prefab and let it do the rest of the logic
/// </summary>
[CreateAssetMenu(fileName = "New InstantiatedAbility", menuName = "Abilities/InstantiatedAbility")]
public class InstantiatedAbilityScriptable : Ability
{
    [SerializeField] private InstantiatedAbilityMono _standardPrefab;
    [SerializeField] private InstantiatedAbilityMono _evoPrefab;

    public float speed;
    public float damage;
    public int localProjectilesAmount = 1; // How many projectiles are fired in a single burst
    public Character nativeUser;
    public List<InstantiatedAbilityMono> Instances { get; private set; }

    void Awake()
    {
        if (GameData.currentCharacter == nativeUser)
        {
            IsEvolved = true;
        }
    }

    /// <summary>
    /// <para>
    /// This is the method that is called when the ability is activated
    /// </para>
    /// instantiates the prefab and sets the ability to it
    /// </summary>
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
