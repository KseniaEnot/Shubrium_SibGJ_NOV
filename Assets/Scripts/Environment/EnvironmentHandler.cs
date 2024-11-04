using System.Collections;
using FMODUnity;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField] private Gradient _cameraGradient;
    [SerializeField] private GameObject[] _nightObjects;
    [SerializeField] private StudioEventEmitter _ambienceEmitter;
    [SerializeField] private Light[] _lights;
    [SerializeField] private float[] _intensityMultiplier;
    [SerializeField] private float _fadeLightSpeed = 4f;

    private float _intensity = 1f;
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
        StartCoroutine(FadeLight());
    }

    private IEnumerator FadeLight()
    {
        float targetValue = _isNight ? 0f : 1f;

        while (_intensity != targetValue)
        {
            _intensity = Mathf.MoveTowards(_intensity, targetValue, _fadeLightSpeed * Time.deltaTime);
            Camera.main.backgroundColor = _cameraGradient.Evaluate(_intensity);
            foreach (Light l in _lights)
            {
                l.color = _cameraGradient.Evaluate(_intensity);
            }

            _ambienceEmitter.SetParameter("daytime", _intensity);

            yield return null;
        }
    }
}
