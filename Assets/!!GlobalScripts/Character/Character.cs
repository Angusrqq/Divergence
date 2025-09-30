using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

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

    public virtual void Unlock()
    {
        if (!GameData.unlockedCharacters.Contains(this))
        {
            GameData.unlockedCharacters.Add(this);
        }
    }

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
