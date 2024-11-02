using System;
using System.Collections;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    public Action OnReachedDestination;

    [SerializeField] private CharacterConfig _config;
    [SerializeField] private Animator _animator;
    private Transform _destination;

    public CharacterConfig Config => _config;
    public Animator Animator => _animator;

    public void SetDestination(Transform destination, bool rotateBeforeMove, bool rotateAfterMove)
    {
        _destination = destination;
        StartCoroutine(MoveTowardsDestination(rotateBeforeMove, rotateAfterMove));
    }

    private IEnumerator MoveTowardsDestination(bool rotateBeforeMove, bool rotateAfterMove)
    {
        if (rotateBeforeMove)
        {
            while (transform.forward != _destination.forward)
            {
                transform.forward = Vector3.MoveTowards(transform.forward, _destination.forward, Time.deltaTime);
                yield return null;
            }
        }
        _animator.SetBool("IsMoving", true);
        while (transform.position != _destination.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination.position, _config.MoveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        if (rotateAfterMove)
        {
            while (transform.forward != _destination.forward)
            {
                transform.forward = Vector3.MoveTowards(transform.forward, _destination.forward, Time.deltaTime);
                yield return null;
            }
        }
        OnReachedDestination?.Invoke();
    }
}