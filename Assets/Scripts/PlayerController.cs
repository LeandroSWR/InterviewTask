using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    // Movement Controls
    private InputAction movementInput;
    private Vector2 movementVector => movementInput.ReadValue<Vector2>().normalized;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementInput = GetComponent<PlayerInput>().actions["Movement"];
    }

    private void Move()
    {
        rb.velocity = movementVector * moveSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }
}
