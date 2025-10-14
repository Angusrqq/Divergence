using UnityEngine;

/// <summary>
/// <para>
/// <c>AnimatedEntity</c> is a class for handling the animation of an entity.
/// </para>
/// NOT FULLY IMPLEMENTED YET
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedEntity : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController animatorController;
    public enum AnimationsList { Default }
    public string currentAnimation;

    void Start()
    {
        animatorController = animatorController ? animatorController : animator.runtimeAnimatorController;
    }

    /// <summary>
    /// <para>
    /// <c>SetAnimatorController</c> is a method for setting the animator controller of the entity.
    /// </para>
    /// Speaks for itself, we might need this later (for example enemy evolution or something like that)
    /// </summary>
    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
        animatorController = controller;
    }

    // virtual protected void Update()
    // {
    //     if(animator.GetCurrentAnimatorStateInfo(0).IsName("Default") == false && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
    //     {
    //         ChangeAnimation(AnimationsList.Default);
    //     }
    // }

    /// <summary>
    /// <para>
    /// Changes the animation by enum.
    /// </para>
    /// </summary>
    public virtual void ChangeAnimation(AnimationsList animation)
    {
        if (currentAnimation == animation.ToString()) return;
        
        animator.Play(animation.ToString());
        currentAnimation = animation.ToString();
    }

    /// <summary>
    /// <para>
    /// Changes the animation by string.
    /// </para>
    /// </summary>
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
