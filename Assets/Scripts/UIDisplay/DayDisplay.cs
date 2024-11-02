using TMPro;
using UnityEngine;

public class DayDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;

	private void OnEnable()
	{
		EventBus.OnDayChanged += UpdateText;
	}

	private void OnDisable()
	{
		EventBus.OnDayChanged -= UpdateText;
	}

	private void UpdateText(int value)
	{
		_text.text = value.ToString();
	}
}
