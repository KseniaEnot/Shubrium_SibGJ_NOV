using FMODUnity;
using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float time)
    {
        StartCoroutine(CheckTimer(time));
    }

    private IEnumerator CheckTimer(float time)
    {
        yield return new WaitForSeconds(time);
        RuntimeManager.PlayOneShot(GameManager.StaticInstance.MiniGame.CoinAudioRef, Camera.main.transform.position);
        GameManager.StaticInstance.Currency.AddGold(1);
        _rb.velocity = Vector2.zero;
        GameManager.StaticInstance.MiniGame.SpawnedCoinsCount--;
        gameObject.SetActive(false);
    }
}
