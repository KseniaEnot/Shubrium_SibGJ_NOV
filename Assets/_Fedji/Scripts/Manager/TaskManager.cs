using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private InGameUIManager _inGameUIManager;
    [SerializeField] private QuestSettingsConfig _questSettingsConfig;
    [SerializeField] private CharacterSettingsConfig _characterSettingsConfig;
    private List<CharacterConfig> _charactersWithoutQuest = new();
    private List<TaskData> _tasks = new();

    public List<TaskData> Tasks => _tasks;

    private void OnEnable()
    {
        EventBus.OnDayChanged += RefreshPools;
        EventBus.OnCharacterReachedEnter += OnCharacterReachedEnter;
        EventBus.OnSendCharacterToExit += OnCharacterReachedExit;
    }

    private void OnDisable()
    {
        EventBus.OnDayChanged -= RefreshPools;
        EventBus.OnCharacterReachedEnter -= OnCharacterReachedEnter;
        EventBus.OnSendCharacterToExit -= OnCharacterReachedExit;
    }

    private void RefreshPools(int value)
    {
        _charactersWithoutQuest.Clear();// clear pool
        for (int i = 0; i < _characterSettingsConfig.CharacterConfigs.Count; i++)
        {
            _charactersWithoutQuest.Add(_characterSettingsConfig.CharacterConfigs[i]);// add characters to pool
        }
        for (int i = _tasks.Count - 1; i >= 0; i--)
        {
            _charactersWithoutQuest.Remove(_tasks[i].CurrentCharacter);// remove character from pool
        }
        int questCount = Random.Range(_questSettingsConfig.MinNewQuestsPerDay, _questSettingsConfig.MaxNewQuestsPerDay + 1);// how many new quest appear today
        CharacterConfig tempCharacter;
        for (int i = 0; i < questCount; i++)
        {
            tempCharacter = _charactersWithoutQuest[Random.Range(0, _charactersWithoutQuest.Count)];// get character without quest
            _charactersWithoutQuest.Remove(tempCharacter);// remove from pool
            _tasks.Add(new(tempCharacter, tempCharacter.GetRandomQuest()));// add random quest to task pool
        }
        CheckTasks();
    }

    private void CheckTasks()
    {
        if (_tasks.Count > 0)
        {
            EventBus.SendCharacter(_characterSettingsConfig.CharacterConfigs.IndexOf(_tasks[0].CurrentCharacter));
            EventBus.SendCharacterToEnter();
        }
        else
        {
            _inGameUIManager.ShowSummaryOfDay(string.Empty);
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
        if (result < _questSettingsConfig.MaxPercentToLowResultReaction)
        {
            _inGameUIManager.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetLowGoldReaction());
        }
        else if (result > _questSettingsConfig.MinPercentToHighResultReaction)
        {
            _inGameUIManager.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetHighGoldReaction());
        }
        else
        {
            _inGameUIManager.OnMiniGameCompleted(_tasks[0].CurrentCharacter.GetNormalGoldReaction());
        }
    }

    public void OnCharacterReachedEnter()
    {
        if (_tasks[0].QuestStarted)
        {
            // сообщает успешно или нет
            if (_tasks[0].QuestSuccessful)
            {
                _inGameUIManager.ShowQuestResultBar($"{_tasks[0].CurrentQuest.SuccessText} {1000f} золота.");// calculate reward!
            }
            else
            {
                _inGameUIManager.ShowQuestResultBar(_tasks[0].CurrentQuest.FailedText);
            }
        }
        else
        {
            _inGameUIManager.ShowQuestRequestBar($"{_tasks[0].CurrentQuest.Description} {_tasks[0].CurrentQuest.RequestedGold} золота.");
        }
    }

    public void OnCharacterReachedExit()
    {
        CheckTasks();
    }
}
