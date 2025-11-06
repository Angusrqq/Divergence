using UnityEngine;

/// <summary>
/// Optional thing to implement
/// </summary>
public class ManuallyTriggeredAbility : Ability
{
    public KeyCode keyCode;

    public override void Activate()
    {
        if (Input.GetKeyDown(keyCode))
        {
            base.Activate();
        }
    }
}
