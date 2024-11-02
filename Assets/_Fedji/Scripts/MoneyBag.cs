using System.Collections;
using UnityEngine;

public class MoneyBag : MonoBehaviour
{
    [Header("For players's bag")]
    [SerializeField] private float _spawnCooldown = 0.5f;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _rotateDownSpeed = 45f;
    [SerializeField] private float _rotateUpSpeed = 90f;
    [SerializeField, Range(45f, 90f)] private float _angleToDropCoins = 60f;
    [SerializeField, Range(90f, 180f)] private float _maxDownAngle = 135f;
    [Header("For character's bag (check true)")]
    [SerializeField] private bool _grabCoins;

    private float _cooldownTime;
    private Coroutine _coroutine;

    public void StartSharing()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(RotateDown());
    }

    public void StopSharing()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(RotateUp());
    }

    private IEnumerator RotateDown()
    {
        float currentAngle;
        while (true)
        {
            currentAngle = transform.eulerAngles.z;
            if (currentAngle < _maxDownAngle)
            {
                transform.Rotate(Vector3.forward * _rotateUpSpeed * Time.deltaTime);
            }
            if (currentAngle > _angleToDropCoins)
            {
                _cooldownTime += Mathf.InverseLerp(_angleToDropCoins, _maxDownAngle, currentAngle) * Time.deltaTime;
                if (_cooldownTime >= _spawnCooldown)
                {
                    if (GameManager.StaticInstance.ReduceGold())
                    {
                        _cooldownTime = 0f;
                        MoneyBagManager.StaticInstance.SpawnedCoinsCount++;
                        Instantiate(_coinPrefab);// assign position
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator RotateUp()
    {
        float currentAngle = transform.eulerAngles.z;
        while (currentAngle > 0f)
        {
            currentAngle = transform.eulerAngles.z;
            transform.Rotate(Vector3.forward * -_rotateUpSpeed * Time.deltaTime);
            if (currentAngle > _angleToDropCoins)
            {
                _cooldownTime += Time.deltaTime;
                if (_cooldownTime >= _spawnCooldown)
                {
                    if (GameManager.StaticInstance.ReduceGold())
                    {
                        _cooldownTime = 0f;
                        MoneyBagManager.StaticInstance.SpawnedCoinsCount++;
                        Instantiate(_coinPrefab);// assign position
                    }
                }
            }
            yield return null;
        }
        _coroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_grabCoins)
        {
            MoneyBagManager.StaticInstance.LootedCoinsCount++;
            MoneyBagManager.StaticInstance.SpawnedCoinsCount--;
            Destroy(collision.gameObject);
            if (MoneyBagManager.StaticInstance.SpawnedCoinsCount == 0)
            {
                MoneyBagManager.StaticInstance.OnAllCoinsCollected?.Invoke(MoneyBagManager.StaticInstance.LootedCoinsCount);
            }
        }
    }
}
