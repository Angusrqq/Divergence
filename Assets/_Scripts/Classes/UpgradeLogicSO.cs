using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeLogic", menuName = "Game/BaseUpgradeLogic")]
public class UpgradeLogicSO : ScriptableObject
{
    public UpgradeScriptable source;
    public virtual void OnBuy()
    {
        source.Level = 1;
    }

    public virtual void OnUpgrade()
    {
        source.Level++;
    }
}