using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private int _deadlineDay = 14;
    [SerializeField, Range(0f, 10f)] private float _timeScale = 1f;
    private int _currentDay = 0;

    public int CurrentDay => _currentDay;
    public int DeadlineDay => _deadlineDay;

    private void OnEnable()
    {
        EventBus.OnNextDay += NextDay;
    }

    private void OnDisable()
    {
        EventBus.OnNextDay -= NextDay;
    }

    private void Update()
    {
        Time.timeScale = _timeScale;
    }

    private void NextDay()
    {
        _currentDay++;
        UpdateDay();
    }

    private void UpdateDay()
    {
        EventBus.TimeOfDayChanged(false);
        if (_currentDay == _deadlineDay)
        {
            EventBus.DeadlineReached();
        }
        else if (!GameManager.StaticInstance.Currency.GameOver)
        {
            EventBus.DayChanged(CurrentDay);
        }
    }
}
