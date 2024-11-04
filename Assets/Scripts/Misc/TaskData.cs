using UnityEngine;

[System.Serializable]
public class TaskData
{
    public CharacterConfig CurrentCharacter;
    public QuestConfig CurrentQuest;
    public bool QuestStarted;
    public bool QuestSuccessful;
    public int RequestedGold;

    public int MinGoldToStopSharingGold => Mathf.FloorToInt(RequestedGold * CurrentQuest.MinGoldPercentToStopSharingGold);

    public TaskData(CharacterConfig currentCharacter, QuestConfig currentQuest)
    {
        CurrentCharacter = currentCharacter;
        CurrentQuest = currentQuest;
        QuestStarted = false;
        QuestSuccessful = false;
    }

    public void RollQuestStateIsSuccessful(float result)
    {
        result = Mathf.Clamp(result, CurrentQuest.MinSuccessChance, CurrentQuest.MaxSuccessChance);
        if (result >= Random.value)
        {
            QuestSuccessful = true;
        }
    }
}
