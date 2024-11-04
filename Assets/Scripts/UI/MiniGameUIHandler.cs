using TMPro;
using UnityEngine;

public class MiniGameUIHandler : MonoBehaviour
{
	[SerializeField] private string _requestedString = "Requested Coins: ";
	[SerializeField] private string _lootedString = "Looted Coins: ";
	[SerializeField] private TextMeshProUGUI _requestedCoinsText;
	[SerializeField] private TextMeshProUGUI _lootedCoinsText;

	private void OnEnable()
	{
		EventBus.OnRequiredCoinsUpdated += DrawRequestedCoins;
		EventBus.OnLootedCoinsUpdated += DrawLootedCoins;
	}

	private void OnDisable()
	{
		EventBus.OnRequiredCoinsUpdated -= DrawRequestedCoins;
		EventBus.OnLootedCoinsUpdated -= DrawLootedCoins;
	}

	private void DrawRequestedCoins(int value)
	{
		_requestedCoinsText.text = $"{_requestedString}{value}";
	}

	private void DrawLootedCoins(int value)
	{
        _lootedCoinsText.text = $"{_lootedString}{value}";
	}
}
