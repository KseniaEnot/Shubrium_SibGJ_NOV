using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Settings", menuName = "Configs/New Quest Settings")]
public class QuestSettingsConfig : ScriptableObject
{
    public List<CharacterConfig> CharacterConfigs = new();
    [Range(1, 10)] public int MinNewQuestsPerDay = 4;
    [Range(1, 10)] public int MaxNewQuestsPerDay = 6;
    [Range(0f, 0.49f)] public float MaxPercentToLowResultReaction = 0.33f;
    [Range(0.51f, 1f)] public float MinPercentToHighResultReaction = 0.66f;
}