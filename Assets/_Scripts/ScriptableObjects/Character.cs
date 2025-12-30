using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>Character</c> is a ScriptableObject that represents a character that can be played by the player.
/// </para>
/// It contains character data such as name, description, icon, animator controller, unlock cost, starting abilities, stats and methods to unlock and upgrade the character.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
public class Character : BaseScriptableObjectUnlockable //TODO: open to suggestions on how do we store character data and if we need any methods here
{
    [Header("Character Unlock")]
    [SerializeField] private int _unlockCost;
    [SerializeField] private int _startLevel;

    [Header("Character Stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _damageScale;
    [SerializeField] private float _cooldownReduction;

    [Header("Character Animations")]
    [SerializeField] private RuntimeAnimatorController _characterAnimatorController;

    [Header("Starting Abilities")]
    [SerializeField] private List<BaseAbilityScriptable> _startingAbilities;
    
    public int UnlockCost => _unlockCost;
    public int StartLevel => _startLevel;
    public int MaxHealth => _maxHealth;
    public float MovementSpeed => _movementSpeed;
    public float DamageScale => _damageScale;
    public float CooldownReduction => _cooldownReduction;
    public RuntimeAnimatorController CharacterAnimatorController => _characterAnimatorController;
    public List<BaseAbilityScriptable> StartingAbilities => _startingAbilities;

    public virtual void Unlock()
    {
        if (!GameData.unlockedCharacters.Contains(this))
        {
            GameData.unlockedCharacters.Add(this);
        }
    }

    // TODO: Implement this method
    public virtual void Upgrade()
    {
        throw new System.NotImplementedException();
        // maxHealth = Mathf.RoundToInt(maxHealth * 1.2f);
        // moveSpeed *= 1.1f;
        // damageScale *= 1.1f;
        // cooldownReduction += 0.05f;
        // magnetRadius += 0.05f;
    }
}
