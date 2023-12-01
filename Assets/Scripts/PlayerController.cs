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
    
    // Movement Keyboard Controls
    private InputAction movementInput;
    private Vector2 movementVector => movementInput.ReadValue<Vector2>().normalized;
    private bool isMoving => movementVector != Vector2.zero;

    // Movement Mouse Controls
    private Vector2 clickToMovePos;
    private bool hasClickToMovePos;
    private bool clickToMove;
    private bool holdToMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAgent = GetComponent<NavMeshAgent>();
        playerAgent.updateRotation = false;
        playerAgent.updateUpAxis = false;
        playerAgent.speed = moveSpeed;
        playerAgent.enabled = false;
        movementInput = GetComponent<PlayerInput>().actions["Movement"];
    }

    private void FixedUpdate()
    {
        if (holdToMove)
        {
            ResetMovementVars();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            return;
        }

        if (isMoving)
        {
            ResetMovementVars();
            rb.velocity = movementVector * moveSpeed;
            return;
        }

        if (!rb.isKinematic)
        {
            rb.velocity = Vector2.zero;
        }

        // When we click to move we use the NavMeshAgent to move the player
        if (!clickToMove || !hasClickToMovePos)
        {
            return;
        }

        playerAgent.enabled = true;
        rb.isKinematic = true;

        if (Vector2.Distance(transform.position, clickToMovePos) <= playerAgent.stoppingDistance)
        {
            ResetMovementVars();
        }
    }

    private void ResetMovementVars()
    {
        if (playerAgent.enabled)
        {
            playerAgent.ResetPath();
        }
        playerAgent.enabled = false;
        hasClickToMovePos = false;
        rb.isKinematic = false;
        clickToMove = false;
    }

    private Vector2 GetClickToMovePosition()
    {
        Ray mousePosition = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mousePosition, out RaycastHit hitInfo))
        {
            print(hitInfo.point);
            return new Vector2(hitInfo.point.x, hitInfo.point.y);
        }
        
        return Vector2.zero;
    }

    /// <summary>
    /// When clicking to move set the destination on click
    /// </summary>
    public void OnClickToMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            clickToMove = true;
            playerAgent.enabled = true;
            rb.isKinematic = true;

            clickToMovePos = GetClickToMovePosition();
            if (clickToMovePos == Vector2.zero)
            {
                return;
            }
            hasClickToMovePos = true;
            playerAgent.SetDestination(clickToMovePos);
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
