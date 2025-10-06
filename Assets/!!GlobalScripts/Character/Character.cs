using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;


/// <summary>
/// <para>
/// <c>Character</c> is a ScriptableObject that represents a character that can be played by the player.
/// </para>
/// It contains character data such as name, description, icon, animator controller, unlock cost, starting abilities, stats and methods to unlock and upgrade the character.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject //TODO: open to suggestions on how do we store character data and if we need any methods here
{
    public new string name;
    public string description;
    public Sprite icon;
    public AnimatorController animatorController;
    public List<string> animationClips;
    public int unlockCost;
    public int startLevel = 0;
    public List<Ability> startingAbilities;
    public int maxHealth;
    public float movementSpeed;
    public float damageScale;
    public float cooldownReduction;

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

/// <summary>
/// <para>
/// <c>Awake</c> populates the <c>animationClips</c> List with the names of the animation clips in the <c>animatorController</c>.
/// </para>
/// </summary>
    public virtual void Awake()
    {
        foreach (AnimationClip clip in animatorController.animationClips)
        {
            animationClips ??= new List<string>();
            if (!animationClips.Contains(clip.name))
            {
                animationClips.Add(clip.name);
            }
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
