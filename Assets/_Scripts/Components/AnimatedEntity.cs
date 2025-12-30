using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedEntity : MonoBehaviour
{
    public Animator Animator;
    public RuntimeAnimatorController AnimatorController;
    [NonSerialized] public string CurrentAnimation;

    public enum AnimationsList { Default }

    void Start()
    {
        Animator = Animator != null ? Animator : GetComponent<Animator>();

        AnimatorController = AnimatorController ? AnimatorController : Animator.runtimeAnimatorController;
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        Animator.runtimeAnimatorController = controller;
        AnimatorController = controller;
    }

    public virtual void ChangeAnimation(AnimationsList animation)
    {
        if (CurrentAnimation == animation.ToString()) return;
        
        Animator.Play(animation.ToString());
        CurrentAnimation = animation.ToString();
    }

    public virtual void ChangeAnimation(string animation)
    {
        if (CurrentAnimation == animation) return;

        Animator.Play(animation);
        CurrentAnimation = animation;
    }
}
