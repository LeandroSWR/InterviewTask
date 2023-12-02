using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MouseHandler : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private UnityEvent<IInteractable> OnMouseOverInteractable;
    [SerializeField] private UnityEvent OnMouseExitInteractable;
    [SerializeField] private UnityEvent OnMouseClickInteractable;
    [SerializeField] private UnityEvent<Vector2, bool> OnMouseClickGround;

    [SerializeField] private GameObject interactCanvas;
    
    private bool mouseOverInteractable;
    private bool mouseClicked;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckMouseOver();
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            mouseClicked = true;
        }
    }

    private void CheckMouseOver()
    {
        // Convert mouse position to world position
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Cast a ray from the mouse position with the interaction layer mask
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
        {
            mouseClicked = false;
            ResetMouseOverInteractable();
            return;
        }

        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            // We don't want to interact with triggers
            if (hit.collider.isTrigger)
            {
                return;
            }

            if (!mouseOverInteractable)
            {
                mouseOverInteractable = true;
                OnMouseOverInteractable?.Invoke(interactable);
            }

            if (mouseClicked && !interactCanvas.activeSelf)
            {
                OnMouseClickInteractable?.Invoke();
            }

            mouseClicked = false;
            return;
        }

        ResetMouseOverInteractable();

        if (hit.transform.CompareTag("Ground"))
        {
            if (!interactCanvas.activeSelf)
            {
                ResetMouseOverInteractable();
            }

            if (mouseClicked && !interactCanvas.activeSelf)
            {
                OnMouseClickGround?.Invoke(hit.point, false);
            }

            if (mouseClicked && interactCanvas.activeSelf)
            {
                interactCanvas.SetActive(false);
            }
        }

        mouseClicked = false;
    }

    private void ResetMouseOverInteractable()
    {
        if (mouseOverInteractable)
        {
            mouseOverInteractable = false;
            OnMouseExitInteractable?.Invoke();
        }
    }
}
