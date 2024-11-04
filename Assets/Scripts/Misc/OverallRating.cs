using UnityEngine;

[System.Serializable]
public class OverallRating
{
    public float CurrentRatingMultiplier;

    public OverallRating()
    {
        CurrentRatingMultiplier = 1f;
    }
    public void AddRating(float result)
    {
        CurrentRatingMultiplier += GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingAddValue *
            Mathf.Clamp(result,
            GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingAddValueMinMultiplier,
            GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingAddValueMaxMultiplier);
        if (CurrentRatingMultiplier > GameManager.StaticInstance.Task.QuestSettingsConfig.MaxOverallRating)
        {
            CurrentRatingMultiplier = GameManager.StaticInstance.Task.QuestSettingsConfig.MaxOverallRating;
        }
    }

    public void ReduceRating()
    {
        CurrentRatingMultiplier -= GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingReduceValue;
        if (CurrentRatingMultiplier < 1f)
        {
            CurrentRatingMultiplier = 1f;
        }
    }
}