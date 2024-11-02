using System;
using UnityEngine;

public class MoneyBagManager : Singleton<MoneyBagManager>
{
    public Action<int> OnAllCoinsCollected;

    public MoneyBag PlayerBag;
    public MoneyBag CharacterBag;

    public int SpawnedCoinsCount;
    public int LootedCoinsCount;
}
