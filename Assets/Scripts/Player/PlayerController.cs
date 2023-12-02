using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private NavMeshAgent playerAgent;
    private Camera mainCamera;

    // Movement Keyboard Controls
    private InputAction movementInput;
    private Vector2 movementVector => movementInput.ReadValue<Vector2>().normalized;
    private bool keysToMove => movementVector != Vector2.zero;

    // Movement Mouse Controls
    private Vector2 clickToMovePos;
    private bool hasClickToMovePos;
    private bool clickToMove;
    private bool holdToMove;
    private bool movingToInteractable;
    public bool isMovingInteractable => movingToInteractable;

    public bool IsMoving => keysToMove || clickToMove || holdToMove;
    public float MovementDirection => rb.isKinematic ? playerAgent.velocity.x : rb.velocity.x;

    public Action OnReachInteractable;
    public Action OnCancelMoveToInteractable;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;

        InitializePlayerAgent();
        movementInput = GetComponent<PlayerInput>().actions["Movement"];
    }

    private void InitializePlayerAgent()
    {
        playerAgent.updateRotation = false;
        playerAgent.updateUpAxis = false;
        playerAgent.speed = moveSpeed;
        playerAgent.enabled = false;
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (holdToMove)
        {
            MoveToMousePosition();
            return;
        }

        if (keysToMove)
        {
            MoveByKeys();
            return;
        }

        if (!rb.isKinematic)
        {
            rb.velocity = Vector2.zero;
        }

        // When we click to move we use the NavMeshAgent to move the player
        if (clickToMove && hasClickToMovePos)
        {
            MoveByClick();
        }
    }

    private void MoveToMousePosition()
    {
        ResetMovementVars();
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        rb.velocity = (mousePosition - (Vector2)transform.position).normalized * moveSpeed;
    }

    private void MoveByKeys()
    {
        ResetMovementVars();
        rb.velocity = movementVector * moveSpeed;
    }

    private void ResetMovementVars()
    {
        if (playerAgent.enabled)
        {
            playerAgent.ResetPath();
        }
        if (movingToInteractable)
        {
            OnCancelMoveToInteractable?.Invoke();
            movingToInteractable = false;
        }
        playerAgent.enabled = false;
        hasClickToMovePos = false;
        rb.isKinematic = false;
        clickToMove = false;
    }

    public void SetClickToMovePosition(Vector2 movePos, bool moveToInteractable)
    {
        movingToInteractable = moveToInteractable;
        clickToMovePos = movePos;
        if (clickToMovePos == Vector2.zero)
        {
            return;
        }
        clickToMove = true;
        playerAgent.enabled = true;
        rb.isKinematic = true;
        hasClickToMovePos = true;
        playerAgent.SetDestination(clickToMovePos);
    }

    private void MoveByClick()
    {
        playerAgent.enabled = true;
        rb.isKinematic = true;

        if (Vector2.Distance(transform.position, clickToMovePos) <= playerAgent.stoppingDistance)
        {
            if (movingToInteractable)
            {
                OnReachInteractable?.Invoke();
            }

            ResetMovementVars();
        }
    }

    /// <summary>
    /// Will automatically return to false when the mouse button is released
    /// </summary>
    public void OnHoldToMove(InputAction.CallbackContext context)
    {
        holdToMove = context.phase == InputActionPhase.Performed;
    }
}
