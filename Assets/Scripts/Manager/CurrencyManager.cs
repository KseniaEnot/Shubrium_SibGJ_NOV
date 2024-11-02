using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private int _gold = 100;
    private int _incomePerDay;
    private int _outcomePerDay;

    public bool ReduceGoldByMiniGame()
    {
        if (_gold > 0)
        {
            _gold--;
            EventBus.GoldChanged(_gold);
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        EventBus.GoldChanged(_gold);
    }

    public void ForceUpdateGoldUI()
    {
        EventBus.GoldChanged(_gold);
    }
}
