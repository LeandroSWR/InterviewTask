using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Hashes
    private readonly int walkTrigger = Animator.StringToHash("Walk");
    private readonly int idleTrigger = Animator.StringToHash("Idle");
    private readonly int interactTrigger = Animator.StringToHash("Interact");

    private Animator animator;
    private PlayerController playerController;

    private bool flipedX = false;
    private bool wasMoving = false;
    private bool wasIdle = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        UpdateMovementAnimation();
        UpdateSpriteDirection();
    }

    private void UpdateMovementAnimation()
    {
        if (playerController.IsMoving && !wasMoving)
        {
            wasIdle = false;
            wasMoving = true;
            animator.SetTrigger(walkTrigger);
        }
        else if (!playerController.IsMoving && !wasIdle)
        {
            wasIdle = true;
            wasMoving = false;
            animator.SetTrigger(idleTrigger);
        }
    }

    private void UpdateSpriteDirection()
    {
        if (playerController.MovementDirection < 0 && !flipedX)
        {
            flipedX = true;
            transform.parent.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerController.MovementDirection > 0 && flipedX)
        {
            flipedX = false;
            transform.parent.localScale = new Vector3(1, 1, 1);
        }
    }
}
