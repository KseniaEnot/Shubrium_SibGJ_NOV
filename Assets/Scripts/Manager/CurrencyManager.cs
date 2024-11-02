using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private int _gold = 100;

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
}
