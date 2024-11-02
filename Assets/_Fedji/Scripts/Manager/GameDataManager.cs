using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public int Gold = 100;
    public QuestSettingsConfig QuestSettingsConfig;
    public List<CharacterConfig> CharacterConfigs = new();

    public bool ReduceGoldByMiniGame()
    {
        if (Gold > 0)
        {
            Gold--;
            return true;
        }
        return false;
    }
}
