using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int _startingGold = 100;
    [SerializeField] private int _requiredGold = 5000;
    [SerializeField, TextArea] private string _loseText = "You died! =(";
    [SerializeField, TextArea] private string _winText = "You win! =)";

    private bool _gameOver;
    private int _currentGold;
    private int _incomePerDay;
    private int _outcomePerDay;
    private int _incomeOverall;
    private int _outcomeOverall;

    public bool GameOver => _gameOver;
    public int CurrentGold => _currentGold;
    public int RequiredGold => _requiredGold;
    public int IncomePerDay => _incomePerDay;
    public int OutcomePerDay => _outcomePerDay;
    public int IncomeOverall => _incomeOverall;
    public int OutcomeOverall => _outcomeOverall;

    private void Awake()
    {
        _currentGold = _startingGold;
    }

    private void OnEnable()
    {
        EventBus.OnNextDay += OnDayChanged;
    }

    private void OnDisable()
    {
        EventBus.OnNextDay -= OnDayChanged;
    }

    private void OnDayChanged()
    {
        if (_incomePerDay == 0 && _outcomePerDay == 0 && _currentGold == 0)
        {
            _gameOver = true;
            EventBus.GameOver();
        }
        else
        {
            _incomePerDay = 0;
            _outcomePerDay = 0;
        }
    }

    public bool ReduceGoldByMiniGame()
    {
        if (_currentGold > 0)
        {
            _currentGold--;
            _outcomePerDay--;
            _outcomeOverall--;
            EventBus.GoldChanged(_currentGold);
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        _currentGold += amount;
        _incomePerDay += amount;
        _incomeOverall += amount;
        EventBus.GoldChanged(_currentGold);
    }

    public void ForceUpdateGoldUI()
    {
        EventBus.GoldChanged(_currentGold);
    }

    public string GetDeadLineResultText()
    {
        if (_currentGold >= _requiredGold)
        {
            return _winText;
        }
        else
        {
            return _loseText;
        }
    }
}
