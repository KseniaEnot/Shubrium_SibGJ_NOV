using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Configs/New Quest")]
public class QuestConfig : ScriptableObject
{
    public string DisplayName = "Name";
    [TextArea] public string Description = "Description";
}