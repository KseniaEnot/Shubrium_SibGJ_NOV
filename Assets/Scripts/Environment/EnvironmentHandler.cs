using System.Collections;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField] private Gradient _cameraGradient;
    [SerializeField] private GameObject[] _nightObjects;
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
        if (_isNight)
        {
            while (_intensity != 0f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 0f, _fadeLightSpeed * Time.deltaTime);
                Camera.main.backgroundColor = _cameraGradient.Evaluate(_intensity);
                foreach (Light l in _lights)
                {
                    l.color = _cameraGradient.Evaluate(_intensity);
                }
                yield return null;
            }
        }
        else
        {
            while (_intensity != 1f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 1f, _fadeLightSpeed * Time.deltaTime);
                Camera.main.backgroundColor = _cameraGradient.Evaluate(_intensity);
                foreach (Light l in _lights)
                {
                    l.color = _cameraGradient.Evaluate(_intensity);
                }
                yield return null;
            }
        }
    }
}
