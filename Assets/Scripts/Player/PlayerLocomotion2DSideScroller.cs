using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerLocomotion2DSideScroller : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _jumpTime;
    private float _groundedTime;
    private bool _isGrounded;
    private bool _isFacedRight = true;

    public float MoveInput;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField, Range(0.1f, 1f)] private float _timeToJump = 0.25f;
    [SerializeField, Range(0.1f, 1f)] private float _timeToFall = 0.25f;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Vector2 _groundSize;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        _jumpTime = _timeToJump;
    }

    private void Update()
    {
        _isGrounded = _rb.velocity.y <= 0f && Physics2D.OverlapBox(_groundChecker.position, _groundSize, 0f, WorldLayerManager.StaticInstance.ObstacleMask);
        if (_isGrounded)
        {
            _groundedTime = _timeToFall;
        }
        else
        {
            _groundedTime -= Time.deltaTime;
        }
        _jumpTime -= Time.deltaTime;
        MoveInput = Input.GetAxis("Horizontal");// временно
        if (Input.GetButtonDown("Jump"))// временно
        {
            Jump();
        }
        Flip();
    }

    private void FixedUpdate()
    {
        if (_jumpTime > 0f && _groundedTime > 0f)
        {
            ApplyJump();
        }
        _rb.velocity = new(MoveInput * _moveSpeed, _rb.velocity.y);
    }

    private void ApplyJump()
    {
        _jumpTime = 0f;
        _groundedTime = 0f;
        _rb.velocity = new(_rb.velocity.x, _jumpForce);
    }

    private void Flip()
    {
        if (_isFacedRight && MoveInput < 0f)
        {
            _isFacedRight = false;
            transform.localScale = new(-1f, 1f, 1f);
        }
        else if (!_isFacedRight && MoveInput > 0f)
        {
            _isFacedRight = true;
            transform.localScale = new(1f, 1f, 1f);
        }
    }
}
