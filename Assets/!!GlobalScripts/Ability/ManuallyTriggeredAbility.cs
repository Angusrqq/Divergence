using UnityEngine;

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
