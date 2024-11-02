using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Configs/New Quest")]
public class QuestConfig : ScriptableObject
{
    public string DisplayName = "Name";
    [TextArea] public string Description = "Problem appear. We need more food. Help me.";
    [TextArea] public string RequestText = "I need/We need/Give me/other.";
    public int RequestedGold = 10;
    [Header("Success Chance")]
    [Range(0f, 1f)] public float MinSuccessChance = 0.25f;
    [Range(0f, 1f)] public float MaxSuccessChance = 0.75f;
    [Header("Max Gold Multiplier Gained From Mini Game For Maximaze Success Chance")]
    [Range(1.5f, 2f)] public float MaxGoldMultiplier = 2f;
}