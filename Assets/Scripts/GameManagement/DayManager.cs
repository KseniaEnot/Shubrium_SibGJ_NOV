using UnityEngine;

public class DayManager : MonoBehaviour
{
	[SerializeField] private int _deadlineDay = 14;
	private readonly int _daysInWeek = 7;
	private int _currentDay = 1;

	public int CurrentDay => _currentDay;

	private void Start()
	{
		UpdateDay();
	}

	public void NextDay()
	{
		_currentDay++;
		UpdateDay();
	}

	private void UpdateDay()
	{
		EventBus.DayChanged(CurrentDay);

		if (_currentDay % _daysInWeek == 0)
		{
			EventBus.WeekCompleted(_currentDay / _daysInWeek);
		}

		if (_currentDay == _deadlineDay)
		{
			EventBus.DeadlineReached();
		}
	}

	public void ResetDays()
	{
		_currentDay = 1;
		UpdateDay();
	}
}
