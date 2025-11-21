using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveAbilityMono : MonoBehaviour
{
    public List<AudioClip> AudioClips;
    public virtual void Activate() { }
    public virtual void Upgrade() { }
    public virtual void UpdateBehaviour() { }
}
