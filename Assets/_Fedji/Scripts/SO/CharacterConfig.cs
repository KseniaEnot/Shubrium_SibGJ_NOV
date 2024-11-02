using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Configs/New Character")]
public class CharacterConfig : ScriptableObject
{
    public string DisplayName = "Name";
    [TextArea] public string Description = "Description";
    public GameObject Model;
    [Range(1f, 5f)] public float MoveSpeed = 4f;
    [TextArea] public List<string> NoGoldReactions = new();
    [TextArea] public List<string> LowGoldReactions = new();
    [TextArea] public List<string> NormalGoldReactions = new();
    [TextArea] public List<string> HighGoldReactions = new();
    public List<QuestConfig> Quests = new();

    public string GetNoGoldReaction()
    {
        return NoGoldReactions[Random.Range(0, NoGoldReactions.Count)];
    }

    public string GetLowGoldReaction()
    {
        return LowGoldReactions[Random.Range(0, LowGoldReactions.Count)];
    }

    public string GetNormalGoldReaction()
    {
        return NormalGoldReactions[Random.Range(0, NormalGoldReactions.Count)];
    }

    public string GetHighGoldReaction()
    {
        return HighGoldReactions[Random.Range(0, HighGoldReactions.Count)];
    }

    public QuestConfig GetRandomQuest()
    {
        return Quests[Random.Range(0, Quests.Count)];
    }
}