using System;

public static class EventBus
{
    public static Action OnGameStarted;
    public static Action OnNextDay;
    public static Action<int> OnDayChanged;
    public static Action<int> OnGoldChanged;
    public static Action<int> OnSendCharacter;
    public static Action OnSendCharacterToEnter;
    public static Action OnSendCharacterToExit;
    public static Action OnCharacterReachedEnter;
    public static Action OnCharacterReachedExit;

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
}
