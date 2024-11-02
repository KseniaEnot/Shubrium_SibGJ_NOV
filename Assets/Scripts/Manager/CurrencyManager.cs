using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    [SerializeField] private int _requiredGold = 5000;
    [SerializeField, TextArea] private string _loseText = "You died! =(";
    [SerializeField, TextArea] private string _winText = "You win! =)";

    private int _gold = 100;
    private int _incomePerDay;
    private int _outcomePerDay;
    private int _incomeOverall;
    private int _outcomeOverall;

    public int RequiredGold => _requiredGold;
    public int IncomePerDay => _incomePerDay;
    public int OutcomePerDay => _outcomePerDay;
    public int IncomeOverall => _incomeOverall;
    public int OutcomeOverall => _outcomeOverall;

    private void OnEnable()
    {
        EventBus.OnDayChanged += OnDayChanged;
    }

    private void OnDayChanged(int obj)
    {
        _incomePerDay = 0;
        _outcomePerDay = 0;
    }

    public bool ReduceGoldByMiniGame()
    {
        if (_gold > 0)
        {
            _gold--;
            _outcomePerDay--;
            _outcomeOverall--;
            EventBus.GoldChanged(_gold);
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        _incomePerDay += amount;
        _incomeOverall += amount;
        EventBus.GoldChanged(_gold);
    }

    public void ForceUpdateGoldUI()
    {
        EventBus.GoldChanged(_gold);
    }

    public string GetDeadLineResultText()
    {
        if (_gold >= _requiredGold)
        {
            return _winText;
        }
        else
        {
            return _loseText;
        }
    }
}
