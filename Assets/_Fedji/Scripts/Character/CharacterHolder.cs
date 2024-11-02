using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterHolder : Singleton<CharacterHolder>
{
	[SerializeField] private Transform _enterPoint;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private float _moveSpeed = 4f;
	[SerializeField] private float _rotationSpeed = 180f;

	private List<GameObject> _characterModels = new();
	private Animator _animator;

	protected override void Awake()
	{
		base.Awake();

		foreach (var config in GameDataManager.StaticInstance.CharacterConfigs)
		{
			var character = Instantiate(config.Model, _exitPoint.position, Quaternion.identity, transform);
			_characterModels.Add(character);
			character.SetActive(false);
		}
	}

	public void SwitchActiveCharacter(int index)
	{
		for (int i = 0; i < _characterModels.Count; i++)
		{
			_characterModels[i].SetActive(i == index);
		}
		_animator = _characterModels[index].GetComponent<Animator>();
	}

	public void SendToEnter()
	{
		MoveCharacter(_enterPoint.position, _enterPoint.forward, OnEnterReached);
	}

	public void SendToExit()
	{
		MoveCharacter(_exitPoint.position, _exitPoint.forward, OnExitReached);
	}

	private void MoveCharacter(Vector3 targetPosition, Vector3 targetDirection, System.Action onReachAction)
	{
		Sequence moveSequence = DOTween.Sequence();

		float rotationDuration = _rotationSpeed / 360f;
		
		//Look in move direction
		moveSequence.Append(_animator.transform.DOLookAt(targetPosition, rotationDuration));
		moveSequence.AppendCallback(() => _animator.SetBool("IsMoving", true));

		float moveDuration = Vector3.Distance(_animator.transform.position, targetPosition) / _moveSpeed;

		moveSequence.Append(_animator.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear));
		moveSequence.AppendCallback(() => _animator.SetBool("IsMoving", false));

		moveSequence.Append(_animator.transform.DOLookAt(targetDirection, rotationDuration));
		moveSequence.OnComplete(() => onReachAction?.Invoke());
	}



	private void OnEnterReached()
	{
		TaskManager.StaticInstance.OnCharacterReachedEnter();
	}

	private void OnExitReached()
	{
		TaskManager.StaticInstance.OnCharacterReachedExit();
	}
}