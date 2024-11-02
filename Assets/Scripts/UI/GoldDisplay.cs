using TMPro;
using UnityEngine;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _layout;
    [SerializeField] private TextMeshProUGUI _text;
	
	private void OnEnable()
	{
		EventBus.OnGoldChanged += UpdateText;
	}
	
	private void OnDisable()
	{
		EventBus.OnGoldChanged -= UpdateText;
	}

	private void UpdateText(int value)
    {
        _layout.SetActive(true);
        _text.text = value.ToString();
	}
}
