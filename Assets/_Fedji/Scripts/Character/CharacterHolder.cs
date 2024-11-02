using System.Collections;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] private CharacterConfig _config;
    [SerializeField] private Animator _animator;

    public CharacterConfig Config => _config;
    public Animator Animator => _animator;

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
        while (transform.forward != -WaypointsManager.StaticInstance.ExitPoint.forward)// rotate face reverse to door
        {
            transform.forward = Vector3.MoveTowards(transform.forward, -WaypointsManager.StaticInstance.ExitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (transform.position != WaypointsManager.StaticInstance.EnterPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, WaypointsManager.StaticInstance.EnterPoint.position, _config.MoveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        while (transform.forward != WaypointsManager.StaticInstance.EnterPoint.forward)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, WaypointsManager.StaticInstance.EnterPoint.forward, Time.deltaTime);
            yield return null;
        }
        TaskManager.StaticInstance.OnCharacterReachedEnter();
    }

    private IEnumerator MoveTowardsExit()
    {
        while (transform.forward != WaypointsManager.StaticInstance.ExitPoint.forward)// rotate face to door
        {
            transform.forward = Vector3.MoveTowards(transform.forward, WaypointsManager.StaticInstance.ExitPoint.forward, Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", true);
        while (transform.position != WaypointsManager.StaticInstance.ExitPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, WaypointsManager.StaticInstance.ExitPoint.position, _config.MoveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("IsMoving", false);
        TaskManager.StaticInstance.OnCharacterReachedExit();
    }
}