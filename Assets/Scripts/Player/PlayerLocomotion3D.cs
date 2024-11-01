using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion3D : MonoBehaviour
{
    private CharacterController _cc;
    private Vector3 _moveVelocity;
    private Vector3 _fallVelocity;
    private float _jumpTime;
    private float _groundedTime;
    private bool _isGrounded;

    public Vector2 MoveInput;
    public Vector2 LookInput;
    [SerializeField] private float _acceleration = 8f;
    [SerializeField] private float _deceleration = 16f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 25f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField, Range(0.1f, 1f)] private float _timeToJump = 0.25f;
    [SerializeField, Range(0.1f, 1f)] private float _timeToFall = 0.25f;
    [SerializeField] private float _gravity = -20f;

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        _jumpTime = _timeToJump;
    }

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
        HandleRotation();
    }

    private void HandleGravity()
    {
        if (_jumpTime > 0f && _groundedTime > 0f)
        {
            ApplyJump();
        }
        _isGrounded = _fallVelocity.y <= 0f && Physics.SphereCast(transform.position + _cc.center, _cc.radius, Vector3.down, out _, _cc.center.y, WorldLayerManager.StaticInstance.ObstacleMask);
        if (_isGrounded)
        {
            _groundedTime = _timeToFall;
            _fallVelocity.y = _gravity / 10f;
        }
        else
        {
            _groundedTime -= Time.deltaTime;
            _fallVelocity.y += _gravity * Time.deltaTime;
        }
        _jumpTime -= Time.deltaTime;
        _cc.Move(_fallVelocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        if (MoveInput != Vector2.zero)
        {
            _moveVelocity = Vector3.MoveTowards(_moveVelocity, (transform.right * MoveInput.x + transform.forward * MoveInput.y).normalized * _moveSpeed, _acceleration * Time.deltaTime);
        }
        else
        {
            _moveVelocity = Vector3.MoveTowards(_moveVelocity, Vector3.zero, _deceleration * Time.deltaTime);
        }
        _cc.Move(_moveVelocity * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (LookInput.x != 0f)
        {
            transform.Rotate(Vector3.up * LookInput.x * _rotateSpeed * Time.deltaTime);
        }
        if (LookInput.y != 0f)
        {
            // зависит уже от ФПС\ТПС
        }
    }

    private void ApplyJump()
    {
        _jumpTime = 0f;
        _groundedTime = 0f;
        _fallVelocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }
}
