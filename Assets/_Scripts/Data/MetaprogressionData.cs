using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// TODO: Egor - Yo. not implemented at all.
/// Should store data about metaprogression, like time knowledge (some currency like gold), unlocked characters, etc.
/// Should be serializable and saved/loaded using DataSystem.
/// </summary>
[System.Serializable]
public class MetaprogressionData
{
    public int timeKnowledge;
    // some parameters that need to be saved here

    public MetaprogressionData(int timeKnowledge)
    {
        this.timeKnowledge = timeKnowledge;
    }
}
