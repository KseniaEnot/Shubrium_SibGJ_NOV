public class GameManager : Singleton<GameManager>
{
    public DayManager Day;
    public CurrencyManager Currency;
    public InGameUIManager UI;
    public MiniGameManager MiniGame;
    public TaskManager Task;
}
