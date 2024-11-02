using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : Singleton<CharacterHolder>
{
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private float _moveSpeed = 4f;

    private Animator _animator;
    private List<GameObject> _characterModels = new();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < GameDataManager.StaticInstance.QuestSettingsConfig.CharacterConfigs.Count; i++)
        {
            _characterModels.Add(Instantiate(GameDataManager.StaticInstance.QuestSettingsConfig.CharacterConfigs[i]).Model);
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
        while (transform.forward != -_exitPoint.forward)// rotate face reverse to door
        {
            transform.forward = Vector3.MoveTowards(transform.forward, -_exitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (transform.position != _enterPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _enterPoint.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        while (transform.forward != _enterPoint.forward)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, _enterPoint.forward, Time.deltaTime);
            yield return null;
        }
        TaskManager.StaticInstance.OnCharacterReachedEnter();
    }

    private IEnumerator MoveTowardsExit()
    {
        while (transform.forward != _exitPoint.forward)// rotate face to door
        {
            transform.forward = Vector3.MoveTowards(transform.forward, _exitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (transform.position != _exitPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _exitPoint.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        TaskManager.StaticInstance.OnCharacterReachedExit();
    }
}