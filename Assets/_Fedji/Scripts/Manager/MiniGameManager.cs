using System.Collections;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public RectTransform PlayerBag;
    public RectTransform CharacterBag;

    public int SpawnedCoinsCount;
    public int LootedCoinsCount;

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

    public void StartMiniGame()
    {

    }


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
                transform.Rotate(Vector3.forward * _rotateDownSpeed * Time.deltaTime);
            }
            if (currentAngle > _angleToDropCoins)
            {
                _cooldownTime += Mathf.InverseLerp(_angleToDropCoins, _maxDownAngle, currentAngle) * Time.deltaTime;
                if (_cooldownTime >= _spawnCooldown)
                {
                    //if (GameDataManager.StaticInstance.ReduceGoldByMiniGame())
                    //{
                    //    _cooldownTime = 0f;
                    //    SpawnedCoinsCount++;
                    //    Instantiate(_coinPrefab);// assign position
                    //}
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
                _cooldownTime += Mathf.InverseLerp(_angleToDropCoins, _maxDownAngle, currentAngle) * Time.deltaTime;
                if (_cooldownTime >= _spawnCooldown)
                {
                    //if (GameDataManager.StaticInstance.ReduceGoldByMiniGame())
                    //{
                    //    _cooldownTime = 0f;
                    //    SpawnedCoinsCount++;
                    //    Instantiate(_coinPrefab);// assign position
                    //}
                }
            }
            yield return null;
        }
        _coroutine = null;
    }

    private void CollectCoin()
    {
        if (_grabCoins)
        {
            LootedCoinsCount++;
            SpawnedCoinsCount--;
            // destroy coin
            if (SpawnedCoinsCount == 0)
            {
                //TaskManager.StaticInstance.OnAllCoinsLooted(LootedCoinsCount);
            }
        }
    }
}
