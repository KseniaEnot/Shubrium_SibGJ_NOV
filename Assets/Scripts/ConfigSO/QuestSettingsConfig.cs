using UnityEngine;

[CreateAssetMenu(fileName = "Quest Settings", menuName = "Configs/New Quest Settings")]
public class QuestSettingsConfig : ScriptableObject
{
    [Range(1, 10)] public int MinNewQuestsPerDay = 4;
    [Range(1, 10)] public int MaxNewQuestsPerDay = 6;
    [Range(1, 10)] public int TasksLeftToEnableNight = 2;
    [Range(0f, 0.49f)] public float MaxPercentToLowResultReaction = 0.33f;
    [Range(0.51f, 1f)] public float MinPercentToHighResultReaction = 0.66f;

    //Overall Rating
    [Range(0.01f, 2f)] public float OverallRatingMinusCoefficient = 0.5f;
    [Range(0.01f, 2f)] public float OverallRatingPlusCoefficient = 0.3f;
    [Range(1f, 3f)] public float MaxOverallRating = 2f;
    [Range(0.01f, 2f)] public float MinOverallRatingCoefficient = 0.5f;
    [Range(0.01f, 2f)] public float MaxOverallRatingCoefficient = 1.5f;

    //Personal Rating
    [Range(0.01f, 2f)] public float PersonalRatingMinusCoefficient = 0.5f;
    [Range(0.01f, 2f)] public float PersonalRatingPlusCoefficient = 0.3f;
    [Range(1f, 3f)] public float MaxPersonalRating = 2f;
    [Range(0.01f, 2f)] public float MinPersonalRatingCoefficient = 0.5f;
    [Range(0.01f, 2f)] public float MaxPersonalRatingCoefficient = 1.5f;
}