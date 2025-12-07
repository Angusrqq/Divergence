using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveAbilityMono : BaseAbilityMono
{
    public List<AudioClip> AudioClips;
    protected PassiveAbilityHandler Ability;
    public virtual void Activate() { }
    public virtual void Upgrade() { }
    public virtual void UpdateBehaviour() { }

    public void Init(PassiveAbilityHandler ability)
    {
        Ability = ability;
    }

}
