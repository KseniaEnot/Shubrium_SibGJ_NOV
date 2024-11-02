using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerBag;
    [SerializeField] private GameObject _characterBag;

    [SerializeField] private int _spawnedCoinsCount;
    [SerializeField] private int _lootedCoinsCount;//

    [Header("For players's bag")]
    [SerializeField] private float _spawnCooldown = 0.5f;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _rotateDownSpeed = 45f;
    [SerializeField] private float _rotateUpSpeed = 90f;
    [SerializeField, Range(45f, 135f)] private float _angleToDropCoins = 90f;
    [SerializeField, Range(90f, 180f)] private float _maxDownAngle = 180f;

    private bool _requiredCancelCoroutine;
    private float _cooldownTime;
    private List<GameObject> _spawnedCoins = new();

    public void StartMiniGame()
    {
        _playerBag.SetActive(true);
        _characterBag.SetActive(true);
    }

    public void StartSharing()
    {
        _requiredCancelCoroutine = false;
        StartCoroutine(RotateDown());
    }

    public void StopSharing()
    {
        _requiredCancelCoroutine = true;
    }

    private IEnumerator RotateDown()
    {
        float currentAngle;
        bool coinSpawned;
        while (!_requiredCancelCoroutine)
        {
            currentAngle = _playerBag.transform.localEulerAngles.z;
            Debug.Log(currentAngle);
            if (currentAngle < _maxDownAngle)
            {
                _playerBag.transform.Rotate(Vector3.forward * _rotateDownSpeed * Time.deltaTime);
            }
            if (currentAngle > _angleToDropCoins)
            {
                _cooldownTime += Mathf.InverseLerp(_angleToDropCoins, _maxDownAngle, currentAngle) * Time.deltaTime;
                if (_cooldownTime >= _spawnCooldown)
                {
                    if (CurrencyManager.StaticInstance.ReduceGoldByMiniGame())
                    {
                        coinSpawned = false;
                        _cooldownTime = 0f;
                        _spawnedCoinsCount++;
                        foreach (GameObject coin in _spawnedCoins)
                        {
                            if (!coin.activeSelf)
                            {
                                coin.transform.position = _playerBag.transform.position;
                                coin.SetActive(true);
                                coinSpawned = true;
                                break;
                            }
                        }
                        if (!coinSpawned)
                        {
                            _spawnedCoins.Add(Instantiate(_coinPrefab, _playerBag.transform.position, Quaternion.identity));
                        }
                    }
                }
            }
            yield return null;
        }
        _requiredCancelCoroutine = false;
        StartCoroutine(RotateUp());
    }

    private IEnumerator RotateUp()
    {
        while (_spawnedCoins.Count > 0)
        {
            Debug.Log("Coins " + _spawnedCoins.Count);
            yield return null;
        }
        float currentAngle = _playerBag.transform.localEulerAngles.z;
        //bool coinSpawned;
        while (currentAngle > 0f)
        {
            _playerBag.transform.Rotate(Vector3.forward * -_rotateUpSpeed * Time.deltaTime);
            //if (currentAngle > _angleToDropCoins)
            //{
            //    _cooldownTime += Mathf.InverseLerp(_angleToDropCoins, _maxDownAngle, currentAngle) * Time.deltaTime;
            //    if (_cooldownTime >= _spawnCooldown)
            //    {
            //        if (CurrencyManager.StaticInstance.ReduceGoldByMiniGame())
            //        {
            //            coinSpawned = false;
            //            _cooldownTime = 0f;
            //            _spawnedCoinsCount++;
            //            foreach (GameObject coin in _spawnedCoins)
            //            {
            //                if (!coin.activeSelf)
            //                {
            //                    coin.transform.position = _playerBag.transform.position;
            //                    coin.SetActive(true);
            //                    coinSpawned = true;
            //                    break;
            //                }
            //            }
            //            if (!coinSpawned)
            //            {
            //                _spawnedCoins.Add(Instantiate(_coinPrefab, _playerBag.transform.position, Quaternion.identity));
            //            }
            //        }
            //    }
            //}
            currentAngle = Mathf.Clamp(_playerBag.transform.localEulerAngles.z, 0f, _maxDownAngle);
            Debug.Log(currentAngle);
            yield return null;
        }
        _playerBag.SetActive(false);
        _characterBag.SetActive(false);
        EventBus.AllCoinsLooted(_lootedCoinsCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _lootedCoinsCount++;
        _spawnedCoinsCount--;
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.gameObject.SetActive(false);
        EventBus.LootedCoinsUpdated(_lootedCoinsCount);
    }
}
