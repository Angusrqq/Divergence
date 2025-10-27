
using System;
using UnityEngine;

[Serializable]
public class PassiveAbilityMono : MonoBehaviour
{
    public virtual void Activate() { }
    public virtual void Upgrade() { }
    public virtual void UpdateBehaviour() { }
}
