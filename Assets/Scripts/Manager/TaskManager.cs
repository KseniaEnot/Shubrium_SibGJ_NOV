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
    [SerializeField] private Color _requiredGoldTextColor = Color.yellow;// = "#FFFF00";
    [SerializeField] private Color _incomeGoldTextColor = Color.green;
    [SerializeField] private Color _outcomeGoldTextColor = Color.red;
    [Header("Game Over")]
    [SerializeField] private string _gameOverMessage = "Вы остались ни с чем! Конец игры.";

    public TaskData CurrentTask => _tasksForCurrentDay[0];

    private void OnEnable()
    {
        EventBus.OnDayChanged += RefreshPools;
        EventBus.OnCharacterReachedEnter += OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit += OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted += MiniGameCompleted;
    }

    private void OnDisable()
    {
        EventBus.OnDayChanged -= RefreshPools;
        EventBus.OnCharacterReachedEnter -= OnCharacterReachedEnter;
        EventBus.OnCharacterReachedExit -= OnCharacterReachedExit;
        EventBus.OnAllCoinsLooted -= MiniGameCompleted;
    }

    private void RefreshPools(int value, bool isDeadline, bool isGameOver)
    {
        if(isGameOver)
        {
            ///// REWORK
            GameManager.StaticInstance.UI.ShowGameOverNotification($"{_gameOverMessage}\n" +
            $"Итоговый доход: {GameManager.StaticInstance.Currency.IncomeOverall}\n" +
            $"Итоговый расход: {GameManager.StaticInstance.Currency.OutcomeOverall}");
            return;
        }
        _deadline = isDeadline;
        if(!_deadline)
        {
            if (_characterRatings.Count == 0)
            {
                _overallRating = new();
                for (int i = 0; i < _characterSettingsConfig.CharacterConfigs.Count; i++)
                {
                    _characterRatings.Add(new(_characterSettingsConfig.CharacterConfigs[i]));
                }
            }
            _charactersWithoutQuest.Clear();// clear pool
            for (int i = 0; i < _characterSettingsConfig.CharacterConfigs.Count; i++)
            {
                _charactersWithoutQuest.Add(_characterSettingsConfig.CharacterConfigs[i]);// add characters to pool
            }
        }
        foreach (TaskData task in _tasksForNextDay)
        {
            _tasksForCurrentDay.Add(task);
        }
        if (!_deadline)
        {
            for (int i = _tasksForCurrentDay.Count - 1; i >= 0; i--)
            {
                _charactersWithoutQuest.Remove(_tasksForCurrentDay[i].CurrentCharacter);// remove character from pool
            }
            int questCount = Random.Range(_questSettingsConfig.MinNewQuestsPerDay, _questSettingsConfig.MaxNewQuestsPerDay + 1);// how many new quest appear today
            CharacterConfig tempCharacter;
            for (int i = _tasksForNextDay.Count; i < questCount + _tasksForNextDay.Count; i++)
            {
                tempCharacter = _charactersWithoutQuest[Random.Range(0, _charactersWithoutQuest.Count)];// get character without quest
                _charactersWithoutQuest.Remove(tempCharacter);// remove from pool
                _tasksForCurrentDay.Add(new(tempCharacter, tempCharacter.GetRandomQuest()));// add random quest to task pool
                int randomGold = Random.Range(_tasksForCurrentDay[i].CurrentQuest.MinRequestedGold, _tasksForCurrentDay[i].CurrentQuest.MaxRequestedGold + 1);
                Debug.Log($"{randomGold} * {_overallRating.CurrentRatingMultiplier} = {Mathf.FloorToInt(randomGold * _overallRating.CurrentRatingMultiplier)}");
                _tasksForCurrentDay[i].RequestedGold = Mathf.FloorToInt(randomGold * _overallRating.CurrentRatingMultiplier);
            }
        }
        _tasksForNextDay.Clear();
        CheckTasks();
    }

    private void CheckTasks()
    {
        if (_tasksForCurrentDay.Count > 0)
        {
            // start coroutine Audio (knock-knock in door, when open, footstep 2 times, when close door)
            // wait up actions than start down actions
            if (!_deadline && _tasksForCurrentDay.Count == _questSettingsConfig.TasksLeftToEnableNight)
            {
                EventBus.TimeOfDayChanged(true);
            }
            EventBus.SendCharacter(_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter));
            EventBus.SendCharacterToEnter();
        }
        else if (!_deadline)
        {
            GameManager.StaticInstance.UI.ShowSummaryOfDay($"Доход за день: <b><color=#{ColorUtility.ToHtmlStringRGBA(_incomeGoldTextColor)}>{GameManager.StaticInstance.Currency.IncomePerDay}</color></b>\n" +
                $"Расход за день: <b><color=#{ColorUtility.ToHtmlStringRGBA(_outcomeGoldTextColor)}>{-GameManager.StaticInstance.Currency.OutcomePerDay}</color></b>\n" +
                $"До возврата долга осталось {GameManager.StaticInstance.Day.DeadlineDay - GameManager.StaticInstance.Day.CurrentDay} дней.\n" +
                $"Осталось накопить <b><color=#{ColorUtility.ToHtmlStringRGBA(_requiredGoldTextColor)}>{GameManager.StaticInstance.Currency.RequiredGold - GameManager.StaticInstance.Currency.CurrentGold}</color></b>.");
        }
        else
        {
            ///// REWORK
            GameManager.StaticInstance.UI.ShowDeadlineNotification($"Сейчас имеется: {GameManager.StaticInstance.Currency.CurrentGold}\n" +
            $"Итоговый доход: {GameManager.StaticInstance.Currency.IncomeOverall}\n" +
            $"Итоговый расход: {GameManager.StaticInstance.Currency.OutcomeOverall}\n" +
            $"Сумма долга: {GameManager.StaticInstance.Currency.RequiredGold}\n" +
            $"{GameManager.StaticInstance.Currency.GetDeadLineResultText()}");
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
        EventBus.RequiredCoinsUpdated(_tasksForCurrentDay[0].RequestedGold);
    }

    public void RemoveCurrentTask()
    {
        if (!_tasksForCurrentDay[0].CurrentQuest.IsRepeatable)
        {
            // remove from quest config ?
        }
        _tasksForCurrentDay.RemoveAt(0);
    }
    public void LowerOverallRating() => _overallRating.ReduceRating();
    public void LowerCharacterRatings() => _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].ReduceRating();

    private void MiniGameCompleted(int goldAmount)
    {
        int minGold = 0;
        int maxGold = Mathf.FloorToInt(_tasksForCurrentDay[0].RequestedGold * _tasksForCurrentDay[0].CurrentQuest.MaxGoldMultiplier);
        float result = Mathf.InverseLerp(minGold, maxGold, goldAmount);
        _tasksForCurrentDay[0].RollQuestStateIsSuccessful(result);
        _tasksForNextDay.Add(_tasksForCurrentDay[0]);
        _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].AddRating(result);
        _overallRating.AddRating(result);
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
                int goldGained = Mathf.FloorToInt(_tasksForCurrentDay[0].RequestedGold * Random.Range(_tasksForCurrentDay[0].CurrentQuest.MinRewardPercent, _tasksForCurrentDay[0].CurrentQuest.MaxRewardPercent) + _tasksForCurrentDay[0].RequestedGold * _characterRatings[_characterSettingsConfig.CharacterConfigs.IndexOf(_tasksForCurrentDay[0].CurrentCharacter)].CurrentRating);
                string resStr = _tasksForCurrentDay[0].CurrentQuest.SuccessText.Replace(
                    "{}",
                    $"<b><color=#{ColorUtility.ToHtmlStringRGBA(_requiredGoldTextColor)}>{goldGained} золота</color></b>"
                );

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
            string resStr = _tasksForCurrentDay[0].CurrentQuest.Description.Replace(
                "{}",
                $"<b><color=#{ColorUtility.ToHtmlStringRGBA(_requiredGoldTextColor)}>{_tasksForCurrentDay[0].RequestedGold} золота</color></b>"
            );

            GameManager.StaticInstance.UI.ShowQuestRequestBar(
                _tasksForCurrentDay[0].CurrentCharacter.DisplayName,
                _tasksForCurrentDay[0].CurrentQuest.DisplayName,
                resStr
            );
        }
    }

    private void OnCharacterReachedExit()
    {
        CheckTasks();
    }
}