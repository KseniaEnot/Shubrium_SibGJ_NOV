using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Configs/New Quest")]
public class QuestConfig : ScriptableObject
{
    public string DisplayName = "Name";
    [TextArea] public string Description = "Problem appear. We need more food. Help me.";
    [TextArea] public string RequestText = "I need/We need/Give me/other.";
    public int RequestedGold = 10;
}