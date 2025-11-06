using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public RuntimeAnimatorController AnimatorController;
    public List<string> AnimationClips;
    public int BaseMaxHealth = 100;
    public float BaseMovementSpeed = 9f;
    public float Damage = 1f;
    public int BaseExp = 10;
    public List<Ability> Abilities;
    public float ColliderRadius;
    public Vector2 ColliderOffset;
}
