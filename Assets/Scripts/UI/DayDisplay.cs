using TMPro;
using UnityEngine;

public class DayDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _layout;
	[SerializeField] private string _dayString = "День: ";
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
        _layout.SetActive(true);
        _text.text = _dayString + value.ToString();
    }
}
