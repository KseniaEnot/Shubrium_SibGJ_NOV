using UnityEngine;

[CreateAssetMenu(fileName = "Quest Settings", menuName = "Configs/New Quest Settings")]
public class QuestSettingsConfig : ScriptableObject
{
    [Header("Low To High")]
    [Range(1, 10)] public int TasksLeftToEnableNight = 2;
    [Range(1, 10)] public int MinNewQuestsPerDay = 4;
    [Range(1, 10)] public int MaxNewQuestsPerDay = 6;
    [Header("Speech Reaction To Gained Gold")]
    [Range(0f, 0.49f)] public float MaxPercentToLowResultReaction = 0.33f;
    [Range(0.51f, 1f)] public float MinPercentToHighResultReaction = 0.66f;
    [Header("Overall Rating")]
    [Range(0.01f, 2f)] public float OverallRatingReduceValue = 0.5f;
    [Range(0.01f, 2f)] public float OverallRatingAddValue = 0.3f;
    [Range(1f, 3f)] public float MaxOverallRating = 2f;
    [Range(0.01f, 2f)] public float OverallRatingAddValueMinMultiplier = 0.5f;
    [Range(0.01f, 2f)] public float OverallRatingAddValueMaxMultiplier = 1.5f;
    [Header("Personal Rating")]
    [Range(0.01f, 2f)] public float PersonalRatingReduceValue = 0.5f;
    [Range(0.01f, 2f)] public float PersonalRatingAddValue = 0.3f;
    [Range(1f, 3f)] public float MaxPersonalRating = 2f;
    [Range(0.01f, 2f)] public float PersonalRatingAddValueMinMultiplier = 0.5f;
    [Range(0.01f, 2f)] public float PersonalRatingAddValueMaxMultiplier = 1.5f;
}