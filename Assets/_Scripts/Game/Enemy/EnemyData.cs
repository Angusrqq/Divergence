using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public RuntimeAnimatorController animatorController;
    public List<string> animationClips;
    public int baseMaxHealth = 100;
    public float baseMovementSpeed = 9f;
    public float damage = 1f;
    public int baseExp = 10;
    public List<Ability> abilities;
    public float colliderRadius;
    public Vector2 colliderOffset;
}
