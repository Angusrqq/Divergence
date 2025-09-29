using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject //TODO: open to suggestions on how do we store character data and if we need any methods here
{
    public new string name;
    public string description;
    public Sprite icon;
    public int unlockCost;
    public int startLevel = 0;
    public List<Ability> startingAbilities;
    public int maxHealth;
    public float moveSpeed;
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
        // uhh i forgot what i wanted in awake
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
