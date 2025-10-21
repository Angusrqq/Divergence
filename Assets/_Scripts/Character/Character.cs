using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>Character</c> is a ScriptableObject that represents a character that can be played by the player.
/// </para>
/// It contains character data such as name, description, icon, animator controller, unlock cost, starting abilities, stats and methods to unlock and upgrade the character.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
public class Character : ScriptableObject //TODO: open to suggestions on how do we store character data and if we need any methods here
{
    [Header("Character Data")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

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

    /// <summary>
    /// <para>
    /// <c>Awake</c> populates the <c>animationClips</c> List with the names of the animation clips in the <c>AnimatorController</c>.
    /// </para>
    /// </summary>
    public virtual void Awake()
    {
        foreach (AnimationClip clip in CharacterAnimatorController.animationClips)
        {
            CharacterAnimationClips ??= new List<string>();
            if (!CharacterAnimationClips.Contains(clip.name))
            {
                CharacterAnimationClips.Add(clip.name);
            }
        }
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

    public string Name
    {
        get => _name;
        private set => _name = value;
    }

    public string Description
    {
        get => _description;
        private set => _description = value;
    }

    public Sprite Icon
    {
        get => _icon;
        private set => _icon = value;
    }

    public int UnlockCost
    {
        get => _unlockCost;
        private set => _unlockCost = value;
    }

    public int StartLevel
    {
        get => _startLevel;
        private set => _startLevel = value;
    }

    public int MaxHealth
    {
        get => _maxHealth;
        private set => _maxHealth = value;
    }

    public float MovementSpeed
    {
        get => _movementSpeed;
        private set => _movementSpeed = value;
    }

    public float DamageScale
    {
        get => _damageScale;
        private set => _damageScale = value;
    }

    public float CooldownReduction
    {
        get => _cooldownReduction;
        private set => _cooldownReduction = value;
    }

    public RuntimeAnimatorController CharacterAnimatorController
    {
        get => _characterAnimatorController;
        private set => _characterAnimatorController = value;
    }

    public List<string> CharacterAnimationClips
    {
        get => _characterAnimationClips;
        private set => _characterAnimationClips = value;
    }

    public List<Ability> StartingAbilities
    {
        get => _startingAbilities;
        private set => _startingAbilities = value;
    }
}
