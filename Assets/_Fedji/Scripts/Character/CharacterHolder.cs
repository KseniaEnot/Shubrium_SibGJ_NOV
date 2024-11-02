using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : Singleton<CharacterHolder>
{
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private float _moveSpeed = 4f;

    [SerializeField] private Animator _animator;
    private List<GameObject> _characterModels = new();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < GameDataManager.StaticInstance.CharacterConfigs.Count; i++)
        {
            _characterModels.Add(Instantiate(GameDataManager.StaticInstance.CharacterConfigs[i].Model, _exitPoint.position, Quaternion.identity, transform));
        }
    }

    public void SwitchActiveCharacter(int index)
    {
        for (int i = 0; i < _characterModels.Count; i++)
        {
            _characterModels[i].SetActive(i == index);
        }
        _animator = GetComponentInChildren<Animator>();
    }

    public void SendToEnter()
    {
        StartCoroutine(MoveTowardsEnter());
    }

    public void SendToExit()
    {
        StartCoroutine(MoveTowardsExit());
    }

    private IEnumerator MoveTowardsEnter()
    {
        while (_animator.transform.forward != -_exitPoint.forward)// rotate face reverse to door
        {
            _animator.transform.forward = Vector3.MoveTowards(_animator.transform.forward, -_exitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (_animator.transform.position != _enterPoint.position)
        {
            _animator.transform.position = Vector3.MoveTowards(_animator.transform.position, _enterPoint.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        while (_animator.transform.forward != _enterPoint.forward)
        {
            _animator.transform.forward = Vector3.MoveTowards(_animator.transform.forward, _enterPoint.forward, Time.deltaTime);
            yield return null;
        }
        TaskManager.StaticInstance.OnCharacterReachedEnter();
    }

    private IEnumerator MoveTowardsExit()
    {
        while (_animator.transform.forward != _exitPoint.forward)// rotate face to door
        {
            _animator.transform.forward = Vector3.MoveTowards(_animator.transform.forward, _exitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (_animator.transform.position != _exitPoint.position)
        {
            _animator.transform.position = Vector3.MoveTowards(_animator.transform.position, _exitPoint.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        TaskManager.StaticInstance.OnCharacterReachedExit();
    }
}