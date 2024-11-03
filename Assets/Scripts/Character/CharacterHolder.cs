using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] private CharacterSettingsConfig _characterSettingsConfig;
    [SerializeField] private Transform _enterPoint;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private float _moveSpeed = 4f;
	[SerializeField] private float _rotationSpeed = 180f;

	private List<GameObject> _characterModels = new();
	private Animator _animator;

	private void Awake()
	{
		foreach (var config in _characterSettingsConfig.CharacterConfigs)
		{
			var character = Instantiate(config.Model, _exitPoint.position, Quaternion.identity, transform);
			_characterModels.Add(character);
			character.SetActive(false);
		}
	}

    private void OnEnable()
    {
		EventBus.OnSendCharacter += SwitchActiveCharacter;
        EventBus.OnSendCharacterToEnter += SendToEnter;
        EventBus.OnSendCharacterToExit += SendToExit;
    }

    private void OnDisable()
    {
        EventBus.OnSendCharacter -= SwitchActiveCharacter;
        EventBus.OnSendCharacterToEnter -= SendToEnter;
        EventBus.OnSendCharacterToExit -= SendToExit;
    }

    private void SwitchActiveCharacter(int index)
	{
		for (int i = 0; i < _characterModels.Count; i++)
		{
			_characterModels[i].SetActive(i == index);
		}
		_animator = _characterModels[index].GetComponent<Animator>();
	}

	private void SendToEnter()
	{
		MoveCharacter(_enterPoint.position, _enterPoint.forward, OnEnterReached);
	}

    private void SendToExit()
	{
		MoveCharacter(_exitPoint.position, _exitPoint.forward, OnExitReached);
	}

	private void MoveCharacter(Vector3 targetPosition, Vector3 targetDirection, System.Action onReachAction)
	{
		Sequence moveSequence = DOTween.Sequence();

		float rotationDuration = _rotationSpeed / 360f;

		Vector3 lookDirection = targetPosition - _animator.transform.position;
		Quaternion targetRotation = Quaternion.LookRotation(lookDirection, _animator.transform.up);
		moveSequence.Append(_animator.transform.DOLocalRotateQuaternion(targetRotation, rotationDuration));
		moveSequence.AppendCallback(() => _animator.SetBool("IsMoving", true));

		float moveDuration = Vector3.Distance(_animator.transform.position, targetPosition) / _moveSpeed;
		moveSequence.Append(_animator.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear));
		moveSequence.AppendCallback(() => _animator.SetBool("IsMoving", false));

		Quaternion finalRotation = Quaternion.LookRotation(targetDirection, _animator.transform.up);
		moveSequence.Append(_animator.transform.DOLocalRotateQuaternion(finalRotation, rotationDuration));
		moveSequence.OnComplete(() => onReachAction?.Invoke());
	}


	private void OnEnterReached()
	{
		EventBus.CharacterReachedEnter();
	}

	private void OnExitReached()
    {
        EventBus.CharacterReachedExit();
    }
}