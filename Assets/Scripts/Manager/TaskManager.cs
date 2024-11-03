using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private QuestSettingsConfig _questSettingsConfig;
    [SerializeField] private CharacterSettingsConfig _characterSettingsConfig;
    private List<CharacterConfig> _charactersWithoutQuest = new();
    [SerializeField] private List<TaskData> _tasksForCurrentDay = new();
    [SerializeField] private List<TaskData> _tasksForNextDay = new();
    private bool _deadline;
    public QuestSettingsConfig QuestSettingsConfig => _questSettingsConfig;
    [SerializeField] private OverallRating _overallRating;
    [SerializeField] private List<CharacterRating> _characterRatings = new();

    public TaskData CurrentTask => _tasksForCurrentDay[0];

    private void OnEnable()
    {
        EventBus.OnDeadlineReached += OnReachedDeadline;
        EventBus.OnDayChanged += RefreshPools;
        EventBus.OnCharacterReachedEnter += OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit += OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted += MiniGameCompleted;
        EventBus.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventBus.OnDeadlineReached -= OnReachedDeadline;
        EventBus.OnDayChanged -= RefreshPools;
        EventBus.OnCharacterReachedEnter -= OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit -= OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted -= MiniGameCompleted;
        EventBus.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        _deadline = true;
        GameManager.StaticInstance.UI.ShowGameOverNotification();
    }

    private void OnReachedDeadline()
    {
        _deadline = true;
        GameManager.StaticInstance.UI.ShowDeadlineNotification();
    }

    private void RefreshPools(int value)
    {
        if (_characterRatings.Count == 0)
        {
            _overallRating = new();
            for (int i = 0; i < _characterSettingsConfig.CharacterConfigs.Count; i++)
            {
                _characterRatings.Add(new(_characterSettingsConfig.CharacterConfigs[i]));
            }
        }

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
            _tasksForCurrentDay[i].RequestedGold = Mathf.RoundToInt(_tasksForCurrentDay[i].CurrentQuest.RequestedGold * _overallRating.OverallRatingCoefficient);
        }
        CheckTasks();
    }

    private void CheckTasks()
    {
        if (_tasksForCurrentDay.Count > 0)
        {
            if (_tasksForCurrentDay.Count == _questSettingsConfig.TasksLeftToEnableNight)
            {
                EventBus.TimeOfDayChanged(true);
            }
            EventBus.SendCharacter(_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter));
            EventBus.SendCharacterToEnter();
        }
        else
        {
            GameManager.StaticInstance.UI.ShowSummaryOfDay($"Доход за день: {GameManager.StaticInstance.Currency.IncomePerDay}\n" +
                $"Расход за день: {GameManager.StaticInstance.Currency.OutcomePerDay}\n" +
                $"До возврата долга осталось {GameManager.StaticInstance.Day.DeadlineDay - GameManager.StaticInstance.Day.CurrentDay} дней.\n" +
                $"Осталось накопить {GameManager.StaticInstance.Currency.RequiredGold - GameManager.StaticInstance.Currency.CurrentGold}.");
            // or EventBus.DayEnded?
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
        EventBus.RequiredCoinsUpdated(_tasksForCurrentDay[0].CurrentQuest.RequestedGold);
    }

    public void RemoveCurrentTask()
    {
        if (!_tasksForCurrentDay[0].CurrentQuest.IsRepeatable)
        {
            // remove from quest config ?
        }
        _tasksForCurrentDay.RemoveAt(0);
    }
    public void LowerOverallRating() => _overallRating.LowerRating();
    public void LowerCharacterRatings() => _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].LowerRating();

    private void MiniGameCompleted(int goldAmount)
    {
        int minGold = 0;
        int maxGold = Mathf.FloorToInt(_tasksForCurrentDay[0].CurrentQuest.RequestedGold * _tasksForCurrentDay[0].CurrentQuest.MaxGoldMultiplier);
        float result = Mathf.InverseLerp(minGold, maxGold, goldAmount);
        _tasksForCurrentDay[0].RollQuestStateIsSuccessful(result);
        _tasksForNextDay.Add(_tasksForCurrentDay[0]);
        _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].UpRating(result);
        _overallRating.UpRating(result);
        if (result < _questSettingsConfig.MaxPercentToLowResultReaction)
        {
            GameManager.StaticInstance.UI.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                _tasksForCurrentDay[0].CurrentCharacter.GetLowGoldReaction());
        }
        else if (result > _questSettingsConfig.MinPercentToHighResultReaction)
        {
            GameManager.StaticInstance.UI.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                _tasksForCurrentDay[0].CurrentCharacter.GetHighGoldReaction());
        }
        else
        {
            GameManager.StaticInstance.UI.OnMiniGameCompleted(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
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
                int goldGained = Mathf.FloorToInt(_tasksForCurrentDay[0].RequestedGold * Random.Range(_tasksForCurrentDay[0].CurrentQuest.MinRewardPercent, _tasksForCurrentDay[0].CurrentQuest.MaxRewardPercent) + _tasksForCurrentDay[0].RequestedGold * _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].PersonalRatingCoefficient);
                string resStr = _tasksForCurrentDay[0].CurrentQuest.SuccessText.Replace("{}", goldGained + " золота");
                GameManager.StaticInstance.UI.ShowQuestResultBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                    resStr);
                GameManager.StaticInstance.Currency.AddGold(goldGained);
            }
            else
            {
                GameManager.StaticInstance.UI.ShowQuestResultBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName, _tasksForCurrentDay[0].CurrentQuest.FailedText);
            }
        }
        else
        {
            string resStr = _tasksForCurrentDay[0].CurrentQuest.Description.Replace("{}", _tasksForCurrentDay[0].RequestedGold + " золота");
            GameManager.StaticInstance.UI.ShowQuestRequestBar(_tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                    _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                    resStr);
        }
    }

    private void OnCharacterReachedExit()
    {
        CheckTasks();
    }
}

[System.Serializable]
public class OverallRating
{
    public float OverallRatingCoefficient;

    public OverallRating()
    {
        OverallRatingCoefficient = 1f;
    }
    public void UpRating(float result)
    {
        OverallRatingCoefficient += GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingPlusCoefficient * Mathf.Clamp(result,
                        GameManager.StaticInstance.Task.QuestSettingsConfig.MinOverallRatingCoefficient,
                        GameManager.StaticInstance.Task.QuestSettingsConfig.MaxOverallRatingCoefficient);
        if (OverallRatingCoefficient > GameManager.StaticInstance.Task.QuestSettingsConfig.MaxOverallRating)
            OverallRatingCoefficient = GameManager.StaticInstance.Task.QuestSettingsConfig.MaxOverallRating;
    }

    public void LowerRating()
    {
        OverallRatingCoefficient -= GameManager.StaticInstance.Task.QuestSettingsConfig.OverallRatingMinusCoefficient;
        if (OverallRatingCoefficient < 1f)
            OverallRatingCoefficient = 1f;
    }
}