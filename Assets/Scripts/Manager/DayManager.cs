using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private int _deadlineDay = 14;
    private readonly int _daysInWeek = 7;
    private int _currentDay = 0;

    public int CurrentDay => _currentDay;

    private void OnEnable()
    {
        EventBus.OnNextDay += NextDay;
    }

    private void OnDisable()
    {
        EventBus.OnNextDay -= NextDay;
    }

    private void NextDay()
    {
        _currentDay++;
        UpdateDay();
    }

    private void UpdateDay()
    {
        if (_currentDay % _daysInWeek == 0)
        {
            EventBus.WeekCompleted(_currentDay / _daysInWeek);
        }
        if (_currentDay == _deadlineDay)
        {
            EventBus.DeadlineReached();
        }
        else if (!CurrencyManager.StaticInstance.GameOver)
        {
            EventBus.DayChanged(CurrentDay);
        }
    }
}
