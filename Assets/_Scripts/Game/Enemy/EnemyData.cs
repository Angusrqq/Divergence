using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public RuntimeAnimatorController animatorController;
    public List<string> animationClips;
    public int baseMaxHealth;
    public float baseMovementSpeed;
    public float damage;
    public int baseExp;
    public List<Ability> abilities;
}
