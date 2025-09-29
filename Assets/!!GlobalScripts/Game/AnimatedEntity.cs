using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedEntity : MonoBehaviour
{
    public Animator animator;
    public AnimatorController animatorController;
    public enum AnimationsList { Default }
    public string currentAnimation;
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorController = animator.runtimeAnimatorController as AnimatorController;
    }

    // virtual protected void Update()
    // {
    //     if(animator.GetCurrentAnimatorStateInfo(0).IsName("Default") == false && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
    //     {
    //         ChangeAnimation(AnimationsList.Default);
    //     }
    // }

    public virtual void ChangeAnimation(AnimationsList animation)
    {
        if (currentAnimation == animation.ToString()) return;
        animator.Play(animation.ToString());
        currentAnimation = animation.ToString();
    }

    public virtual void ChangeAnimation(string animation)
    {
        if (currentAnimation == animation) return;
        animator.Play(animation);
        currentAnimation = animation;
    }

    // public virtual IEnumerator PlayAnimation(AnimationsList animation)
    // {
    //     animator.Play(animation.ToString());
    //     yield return null;
    //     while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //     {
    //         yield return null;
    //     }
    //     animator.Play(AnimationsList.Default.ToString());
    // }
}
