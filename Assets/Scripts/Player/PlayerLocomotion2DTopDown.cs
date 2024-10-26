using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerLocomotion2DTopDown : MonoBehaviour
{
    private Rigidbody2D _rb;
    private bool _isFacedRight = true;

    public Vector2 MoveInput;
    [SerializeField] private float _moveSpeed = 4f;

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        _rb.velocity = MoveInput.normalized * _moveSpeed;
    }

    private void Flip()
    {
        if (_isFacedRight && MoveInput.x < 0f)
        {
            _isFacedRight = false;
            transform.localScale = new(-1f, 1f, 1f);
        }
        else if (!_isFacedRight && MoveInput.x > 0f)
        {
            _isFacedRight = true;
            transform.localScale = new(1f, 1f, 1f);
        }
    }
}
