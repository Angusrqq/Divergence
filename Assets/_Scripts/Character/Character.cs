using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>Character</c> is a ScriptableObject that represents a character that can be played by the player.
/// </para>
/// It contains character data such as name, description, icon, animator controller, unlock cost, starting abilities, stats and methods to unlock and upgrade the character.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
public class Character : BaseScriptableObjectInfo //TODO: open to suggestions on how do we store character data and if we need any methods here
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
    [SerializeField] private List<string> _characterAnimationClips;

    [Header("Starting Abilities")]
    [SerializeField] private List<Ability> _startingAbilities;
    public int UnlockCost => _unlockCost;
    public int StartLevel => _startLevel;
    public int MaxHealth => _maxHealth;
    public float MovementSpeed => _movementSpeed;
    public float DamageScale => _damageScale;
    public float CooldownReduction => _cooldownReduction;
    public RuntimeAnimatorController CharacterAnimatorController => _characterAnimatorController;
    public List<string> CharacterAnimationClips
    {
        get => _characterAnimationClips;
        private set => _characterAnimationClips = value;
    }
    public List<Ability> StartingAbilities => _startingAbilities;

    /// <summary>
    /// Initializes the character by populating the <c>CharacterAnimationClips</c> List with the names of the animation clips in the <c>AnimatorController</c>.
    /// If the <c>AnimatorController</c> is null, this method does nothing.
    /// </summary>
    public void Initialize()
    {
        if (CharacterAnimatorController == null) return;
        CharacterAnimationClips ??= new List<string>();
        CharacterAnimationClips.Clear();
        foreach (var clip in CharacterAnimatorController.animationClips)
            CharacterAnimationClips.Add(clip.name);
    }

    /// <summary>
    /// <para>
    /// <c>Unlock</c> adds this character to the <c>GameData.unlockedCharacters</c> List if it is not already present.
    /// </para>
    /// </summary>
    public virtual void Unlock()
    {
        if (!GameData.unlockedCharacters.Contains(this))
        {
            GameData.unlockedCharacters.Add(this);
        }
    }


    public virtual void Upgrade() // wow great autocomplete suggestion, might implement that later
    {
        throw new System.NotImplementedException();
        // startLevel++;
        // maxHealth = Mathf.RoundToInt(maxHealth * 1.2f);
        // moveSpeed *= 1.1f;
        // damageScale *= 1.1f;
        // cooldownReduction += 0.05f;
        // foreach (Ability a in startingAbilities)
        // {
        //     a.Upgrade();
        // }
    }
}
