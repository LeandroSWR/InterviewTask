using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Hashes
    private readonly int isWalking = Animator.StringToHash("Walk");
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
        animator.SetBool(isWalking, playerController.IsMoving);
    }

    private void UpdateSpriteDirection()
    {
        if (playerController.MovementDirection < -1.2f && !flipedX)
        {
            flipedX = true;
            transform.parent.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerController.MovementDirection > 1.2f && flipedX)
        {
            flipedX = false;
            transform.parent.localScale = new Vector3(1, 1, 1);
        }
    }
}
