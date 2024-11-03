using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _nightObjects;
    [SerializeField] private GameObject[] _daytimeObjects;

    private bool _isNight = false;

    private void OnEnable()
    {
        EventBus.OnTimeOfDayChanged += HandleTimeOfDayChange;
    }

    private void OnDisable()
    {
        EventBus.OnTimeOfDayChanged -= HandleTimeOfDayChange;
    }

    private void HandleTimeOfDayChange(bool isNight)
    {
        _isNight = isNight;

        foreach (var nightObj in _nightObjects)
        {
            nightObj.SetActive(_isNight);
        }

        foreach (var dayObj in _daytimeObjects)
        {
            dayObj.SetActive(!_isNight);
        }
    }
}
