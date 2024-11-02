using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public int Gold = 100;
    public QuestSettingsConfig QuestSettingsConfig;

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
