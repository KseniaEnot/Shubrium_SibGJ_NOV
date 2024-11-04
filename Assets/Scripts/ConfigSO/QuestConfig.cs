using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Configs/New Quest")]
public class QuestConfig : ScriptableObject
{
    public string DisplayName = "Name";
    [TextArea] public string Description = "Problem appear. We need more food. Help me. I need";
    [Header("Requested Gold Count")]
    [Range(1, 100)] public int MinRequestedGold = 5;
    [Range(1, 100)] public int MaxRequestedGold = 15;
    [Header("Random Gained Additional Gold When Success Quest")]
    [Range(0f, 10f)] public float MinRewardPercent = 0.25f;
    [Range(0f, 10f)] public float MaxRewardPercent = 0.75f;
    [TextArea] public string SuccessText = "I complete quest and give you";
    [TextArea] public string FailedText = "I fail quest... Sorry... Cant give you gold...";
    public bool IsRepeatable = true;
    [Header("Success Chance")]
    [Range(0f, 1f)] public float MinSuccessChance = 0.25f;
    [Range(0f, 1f)] public float MaxSuccessChance = 0.75f;
    [Header("Max Gold Multiplier Gained From Mini Game For Maximaze Success Chance")]
    [Range(1.5f, 2f)] public float MaxGoldMultiplier = 2f;
    [Range(0.1f, 0.5f)] public float MinGoldPercentToStopSharingGold = 0.25f;
}