using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private InGameUIManager _inGameUIManager;
    [SerializeField] private QuestSettingsConfig _questSettingsConfig;
    [SerializeField] private CharacterSettingsConfig _characterSettingsConfig;
    private List<CharacterConfig> _charactersWithoutQuest = new();
    [SerializeField] private List<TaskData> _tasksForCurrentDay = new();
    [SerializeField] private List<TaskData> _tasksForNextDay = new();
    private bool _deadline;

    private void OnEnable()
    {
        EventBus.OnDeadlineReached += OnReachedDeadline;
        EventBus.OnDayChanged += RefreshPools;
        EventBus.OnCharacterReachedEnter += OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit += OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted += MiniGameCompleted;
    }

    private void OnDisable()
    {
        EventBus.OnDeadlineReached -= OnReachedDeadline;
        EventBus.OnDayChanged -= RefreshPools;
        EventBus.OnCharacterReachedEnter -= OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit -= OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted -= MiniGameCompleted;
    }

    private void OnReachedDeadline()
    {
        _deadline = true;
        _inGameUIManager.ShowDeadlineNotification();
        // invoke???
    }

    private void RefreshPools(int value)
    {
        if (_deadline)
        {
            return;
        }
        _charactersWithoutQuest.Clear();// clear pool
        for (int i = 0; i < _characterSettingsConfig.CharacterConfigs.Count; i++)
        {
            _charactersWithoutQuest.Add(_characterSettingsConfig.CharacterConfigs[i]);// add characters to pool
        }
        foreach (TaskData task in _tasksForNextDay)
        {
            _tasksForCurrentDay.Add(task);
        }
        for (int i = _tasksForCurrentDay.Count - 1; i >= 0; i--)
        {
            _charactersWithoutQuest.Remove(_tasksForCurrentDay[i].CurrentCharacter);// remove character from pool
        }
        _tasksForNextDay.Clear();
        int questCount = Random.Range(_questSettingsConfig.MinNewQuestsPerDay, _questSettingsConfig.MaxNewQuestsPerDay + 1);// how many new quest appear today
        CharacterConfig tempCharacter;
        for (int i = 0; i < questCount; i++)
        {
            tempCharacter = _charactersWithoutQuest[Random.Range(0, _charactersWithoutQuest.Count)];// get character without quest
            _charactersWithoutQuest.Remove(tempCharacter);// remove from pool
            _tasksForCurrentDay.Add(new(tempCharacter, tempCharacter.GetRandomQuest()));// add random quest to task pool
        }
        CheckTasks();
    }

    private void CheckTasks()
    {
        if (_tasksForCurrentDay.Count > 0)
        {
            EventBus.SendCharacter(_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter));
            EventBus.SendCharacterToEnter();
        }
        else
        {
            _inGameUIManager.ShowSummaryOfDay($"Income: {CurrencyManager.StaticInstance.IncomePerDay}\nOutcome: {CurrencyManager.StaticInstance.OutcomePerDay}");
        }
    }

    public void GetCurrentTaskNoGoldReactionText(out string characterName, out string questName, out string descriptionText)
    {
        characterName = _tasksForCurrentDay[0].CurrentCharacter.DisplayName;
        questName = _tasksForCurrentDay[0].CurrentQuest.DisplayName;
        descriptionText = _tasksForCurrentDay[0].CurrentCharacter.GetNoGoldReaction();
    }

    public void MarkCurrentTaskAsStarted()
    {
        _tasksForCurrentDay[0].QuestStarted = true;// mark quest as started
    }

    public void RemoveCurrentTask()
    {
        if (!_tasksForCurrentDay[0].CurrentQuest.IsRepeatable)
        {
            // remove from quest config ?
        }
        _tasksForCurrentDay.RemoveAt(0);
    }

    private void MiniGameCompleted(int goldAmount)
    {
        int minGold = 0;
        int maxGold = Mathf.FloorToInt(_tasksForCurrentDay[0].CurrentQuest.RequestedGold * _tasksForCurrentDay[0].CurrentQuest.MaxGoldMultiplier);
        float result = Mathf.InverseLerp(minGold, maxGold, goldAmount);
        _tasksForCurrentDay[0].RollQuestStateIsSuccessful(result);
        _tasksForNextDay.Add(_tasksForCurrentDay[0]);
        if (result < _questSettingsConfig.MaxPercentToLowResultReaction)
        {
            _inGameUIManager.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                _tasksForCurrentDay[0].CurrentCharacter.GetLowGoldReaction());
        }
        else if (result > _questSettingsConfig.MinPercentToHighResultReaction)
        {
            _inGameUIManager.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                _tasksForCurrentDay[0].CurrentCharacter.GetHighGoldReaction());
        }
        else
        {
            _inGameUIManager.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                _tasksForCurrentDay[0].CurrentCharacter.GetNormalGoldReaction());
        }
    }

    private void OnCharacterReachedEnter()
    {
        if (_tasksForCurrentDay[0].QuestStarted)
        {
            if (_tasksForCurrentDay[0].QuestSuccessful)
            {
                _inGameUIManager.ShowQuestResultBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                    $"{_tasksForCurrentDay[0].CurrentQuest.SuccessText} {_tasksForCurrentDay[0].CurrentQuest.RequestedGold * Random.Range(_tasksForCurrentDay[0].CurrentQuest.MinRewardPercent, _tasksForCurrentDay[0].CurrentQuest.MaxRewardPercent) + _tasksForCurrentDay[0].CurrentQuest.RequestedGold} золота.");
            }
            else
            {
                _inGameUIManager.ShowQuestResultBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName, _tasksForCurrentDay[0].CurrentQuest.FailedText);
            }
        }
        else
        {
            _inGameUIManager.ShowQuestRequestBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                    $"{_tasksForCurrentDay[0].CurrentQuest.Description} {_tasksForCurrentDay[0].CurrentQuest.RequestedGold} золота.");
        }
    }

    private void OnCharacterReachedExit()
    {
        CheckTasks();
    }
}
