using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    //private Dictionary<CharacterConfig, bool> _characterVisited = new();
    private List<CharacterConfig> _charactersWithoutQuest = new();
    private List<TaskData> _tasks = new();

    public List<TaskData> Tasks => _tasks;

    protected override void Awake()
    {
        base.Awake();
        //for (int i = 0; i < GameDataManager.StaticInstance.CharacterConfigs.Count; i++)
        //{
        //    _characterVisited.Add(GameDataManager.StaticInstance.CharacterConfigs[i], false);// create pool
        //}
    }

    public void StartNewDay()
    {
        _charactersWithoutQuest.Clear();// clear pool
        for (int i = 0; i < GameDataManager.StaticInstance.CharacterConfigs.Count; i++)
        {
            //_characterVisited[GameDataManager.StaticInstance.CharacterConfigs[i]] = false;// toogle visited pool
            _charactersWithoutQuest.Add(GameDataManager.StaticInstance.CharacterConfigs[i]);// add characters to pool
        }
        for (int i = _tasks.Count - 1; i >= 0; i--)
        {
            _charactersWithoutQuest.Remove(_tasks[i].CurrentCharacter);// remove character from pool
        }
        int questCount = Random.Range(GameDataManager.StaticInstance.QuestSettingsConfig.MinNewQuestsPerDay, GameDataManager.StaticInstance.QuestSettingsConfig.MaxNewQuestsPerDay + 1);// how many new quest appear today
        CharacterConfig tempCharacter;
        for (int i = 0; i < questCount; i++)
        {
            tempCharacter = _charactersWithoutQuest[Random.Range(0, _charactersWithoutQuest.Count)];// get character without quest
            _charactersWithoutQuest.Remove(tempCharacter);// remove from pool
            //_characterVisited[tempCharacter] = true;// toggle visited pool
            _tasks.Add(new(tempCharacter, tempCharacter.GetRandomQuest()));// add random quest to task pool
        }
        CheckTasks();
    }

    private void CheckTasks()
    {
        if (_tasks.Count > 0)
        {
            CharacterHolder.StaticInstance.SwitchActiveCharacter(GameDataManager.StaticInstance.CharacterConfigs.IndexOf(_tasks[0].CurrentCharacter));
            CharacterHolder.StaticInstance.SendToEnter();
        }
        else
        {
            InGameUIManager.StaticInstance.ShowSummaryOfDay();
        }
    }

    public string GetCurrentTaskNoGoldReactionText()
    {
        return _tasks[0].CurrentCharacter.GetNoGoldReaction();
    }

    public void MarkCurrentTaskAsStarted()
    {
        _tasks[0].QuestStarted = true;// mark quest as started
    }

    public void RemoveCurrentTask()
    {
        _tasks.RemoveAt(0);
    }

    public void OnAllCoinsLooted(int goldAmount)
    {
        // maybe any other logic
        MiniGameCompleted(goldAmount);
    }

    private void MiniGameCompleted(int goldAmount)
    {
        int minGold = 0;
        int maxGold = Mathf.FloorToInt(_tasks[0].CurrentQuest.RequestedGold * _tasks[0].CurrentQuest.MaxGoldMultiplier);
        float result = Mathf.InverseLerp(minGold, maxGold, goldAmount);
        _tasks[0].RollQuestStateIsSuccessful(result);
        if (result < GameDataManager.StaticInstance.QuestSettingsConfig.MaxPercentToLowResultReaction)
        {
            InGameUIManager.StaticInstance.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetLowGoldReaction());
        }
        else if (result > GameDataManager.StaticInstance.QuestSettingsConfig.MinPercentToHighResultReaction)
        {
            InGameUIManager.StaticInstance.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetHighGoldReaction());
        }
        else
        {
            InGameUIManager.StaticInstance.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetNormalGoldReaction());
        }
    }

    public void OnCharacterReachedEnter()
    {
        if (_tasks[0].QuestStarted)
        {
            // сообщает успешно или нет
            if (_tasks[0].QuestSuccessful)
            {
                InGameUIManager.StaticInstance.ShowQuestResultBar($"{_tasks[0].CurrentQuest.SuccessText} {1000f} золота.");// calculate reward!
            }
            else
            {
                InGameUIManager.StaticInstance.ShowQuestResultBar(_tasks[0].CurrentQuest.FailedText);
            }
        }
        else
        {
            InGameUIManager.StaticInstance.ShowQuestRequestBar($"{_tasks[0].CurrentQuest.Description} {_tasks[0].CurrentQuest.RequestedGold} золота.");
        }
    }

    public void OnCharacterReachedExit()
    {
        CheckTasks();
    }
}
