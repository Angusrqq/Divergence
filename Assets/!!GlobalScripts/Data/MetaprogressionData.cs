using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
