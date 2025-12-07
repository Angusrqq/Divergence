using UnityEngine;

public class PostGameStat : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text valueText;
    [SerializeField] private PostGameStatType statType;
    [SerializeField] private AbilityIconDisplay abilityIconDisplayPrefab;

    public /*async*/ void OnEnable()
    {
        switch (statType)
        {
            case PostGameStatType.Time:
                SetTime();
                break;
            case PostGameStatType.Level:
                SetLevel();
                break;
            case PostGameStatType.Currency:
                SetCurrency();
                break;
            case PostGameStatType.Kills:
                SetKills();
                break;
            case PostGameStatType.DamageDealt:
                SetDamageDealt();
                break;
            // case PostGameStatType.Abilities:
            //     SetAbilities();
            //     break;
        }
    }

    private /*async*/ void SetTime()
    {
        valueText.text = GameTimer.FormatTimeConditionally(GameData.GameTimerInstance.currentTime);
        GameData.CurrentMetadata.gameStats.TotalTime += (uint)GameData.GameTimerInstance.currentTime;
    }

    private /*async*/ void SetLevel()
    {
        valueText.text = GameData.player.Level.ToString();
        if(GameData.player.Level > GameData.CurrentMetadata.Records.MaxLevel)
        {
            GameData.CurrentMetadata.Records.MaxLevel = (uint)GameData.player.Level;
            //TODO: add feedback (text like PB or something), applies for everything
        }
    }

    private /*async*/ void SetCurrency()
    {
        valueText.text = "0";
        Debug.LogWarning("Currency not implemented yet");
    }

    private /*async*/ void SetKills()
    {
        valueText.text = KillCounter.Kills.ToString();
        GameData.CurrentMetadata.gameStats.TotalKills += (uint)KillCounter.Kills;
        
    }

    private /*async*/ void SetDamageDealt()
    {
        valueText.text = GameData.InGameAttributes.DamageDealt.ToString();
        GameData.CurrentMetadata.gameStats.TotalDamageDealt += (ulong)GameData.InGameAttributes.DamageDealt;
        if(GameData.InGameAttributes.DamageDealt > GameData.CurrentMetadata.Records.MaxDamageDealt)
        {
            GameData.CurrentMetadata.Records.MaxDamageDealt = (uint)GameData.InGameAttributes.DamageDealt;
            //TODO: add feedback (text like PB or something), applies for everything
        }
    }

    // private async void SetAbilities()
    // {
    //     valueText.gameObject.SetActive(false);
    //     if(abilityIconDisplayPrefab != null) 
    //     {
    //         var abilityIconDisplay = Instantiate(abilityIconDisplayPrefab, parent: transform, position: valueText.transform.position, rotation: Quaternion.identity);
    //         abilityIconDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -150f);
    //         await Awaitable.NextFrameAsync();
    //         abilityIconDisplay.UpdateActiveAbilitiesIcons(GameData.player.AbilityHolder.Abilities);
    //         abilityIconDisplay.UpdatePassiveAbilitiesIcons(GameData.player.AbilityHolder.Passives);
    //     }

    // }
}

public enum PostGameStatType
{
    Time,
    Level,
    Currency,
    Kills,
    DamageDealt
    // Abilities
}
