public class GameManager : Singleton<GameManager>
{
    public DayManager Day;
    public CharacterHolder Character;
    public CurrencyManager Currency;
    public InGameUIManager UI;
    public MiniGameManager MiniGame;
    public TaskManager Task;
}
