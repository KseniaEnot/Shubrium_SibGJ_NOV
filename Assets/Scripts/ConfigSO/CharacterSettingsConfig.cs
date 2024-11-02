using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Settings", menuName = "Configs/New Character Settings")]
public class CharacterSettingsConfig : ScriptableObject
{
    public List<CharacterConfig> CharacterConfigs = new();
}