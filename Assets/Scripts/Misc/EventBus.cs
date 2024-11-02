using System;
using UnityEngine;

public static class EventBus
{
    public static event Action OnGameStarted;
    public static event Action OnNextDay;
    public static event Action<int> OnDayChanged;
    public static event Action<int> OnWeekCompleted;
    public static event Action OnDeadlineReached;
    public static event Action<int> OnGoldChanged;
    public static event Action<int> OnSendCharacter;
    public static event Action OnSendCharacterToEnter;
    public static event Action OnSendCharacterToExit;
    public static event Action OnCharacterReachedEnter;
    public static event Action OnCharacterReachedExit;
    public static event Action<int> OnAllCoinsLooted;

    public static void GameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public static void NextDay()
    {
        OnNextDay?.Invoke();
    }

    public static void DayChanged(int value)
    {
        OnDayChanged?.Invoke(value);
    }

    public static void DeadlineReached()
    {
        OnDeadlineReached?.Invoke();
    }
    public static void WeekCompleted(int value)
    {
        OnWeekCompleted?.Invoke(value);
    }

    public static void GoldChanged(int value)
    {
        OnGoldChanged?.Invoke(value);
    }

    public static void SendCharacter(int id)
    {
        OnSendCharacter?.Invoke(id);
    }

    public static void SendCharacterToEnter()
    {
        OnSendCharacterToEnter?.Invoke();
    }

    public static void SendCharacterToExit()
    {
        OnSendCharacterToExit?.Invoke();
    }

    public static void CharacterReachedEnter()
    {
        OnCharacterReachedEnter?.Invoke();
    }

    public static void CharacterReachedExit()
    {
        OnCharacterReachedExit?.Invoke();
    }

    public static void AllCoinsLooted(int amount)
    {
        OnAllCoinsLooted?.Invoke(amount);
    }
}
