using UnityEngine;

[System.Serializable]
public class CharacterRating
{
    public CharacterConfig CurrentCharacter;
    public float CurrentRating;

    public CharacterRating(CharacterConfig _currentCharacter)
    {
        CurrentRating = 1f;
    }

    public void AddRating(float result)
    {
        CurrentRating += GameManager.StaticInstance.Task.QuestSettingsConfig.PersonalRatingAddValue * 
            Mathf.Clamp(result, 
            GameManager.StaticInstance.Task.QuestSettingsConfig.PersonalRatingAddValueMinMultiplier, 
            GameManager.StaticInstance.Task.QuestSettingsConfig.PersonalRatingAddValueMaxMultiplier);
        if (CurrentRating > GameManager.StaticInstance.Task.QuestSettingsConfig.MaxPersonalRating)
        {
            CurrentRating = GameManager.StaticInstance.Task.QuestSettingsConfig.MaxPersonalRating;
        }
    }

    public void ReduceRating()
    {
        CurrentRating -= GameManager.StaticInstance.Task.QuestSettingsConfig.PersonalRatingReduceValue;
        if (CurrentRating < 1f)
        {
            CurrentRating = 1f;
        }
    }
}