using UnityEngine;

/// <summary>
/// <para>
/// <c>AnimatedEntity</c> is a class for handling the animation of an entity.
/// </para>
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedEntity : MonoBehaviour
{
    public Animator Animator;
    public RuntimeAnimatorController AnimatorController;
    public string CurrentAnimation;

    public enum AnimationsList { Default }

    void Start()
    {
        Animator = Animator != null ? Animator : GetComponent<Animator>();
        AnimatorController = AnimatorController ? AnimatorController : Animator.runtimeAnimatorController;
    }

    /// <summary>
    /// <para>
    /// <c>SetAnimatorController</c> is a method for setting the animator controller of the entity.
    /// </para>
    /// Speaks for itself, we might need this later (for example enemy evolution or something like that)
    /// </summary>
    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        Animator.runtimeAnimatorController = controller;
        AnimatorController = controller;
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
        if (CurrentAnimation == animation.ToString()) return;
        
        Animator.Play(animation.ToString());
        CurrentAnimation = animation.ToString();
    }

    /// <summary>
    /// <para>
    /// Changes the animation by string.
    /// </para>
    /// </summary>
    public virtual void ChangeAnimation(string animation)
    {
        if (CurrentAnimation == animation) return;

        Animator.Play(animation);
        CurrentAnimation = animation;
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
