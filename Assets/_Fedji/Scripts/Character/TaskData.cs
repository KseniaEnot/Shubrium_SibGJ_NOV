using UnityEngine;

public class TaskData
{
    public CharacterConfig CurrentCharacter;
    public QuestConfig CurrentQuest;
    public bool QuestStarted;

    public TaskData(CharacterConfig currentCharacter, QuestConfig currentQuest, bool questStarted)
    {
        CurrentCharacter = currentCharacter;
        CurrentQuest = currentQuest;
        QuestStarted = questStarted;
    }
}
