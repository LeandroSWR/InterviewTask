using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Handle displaying the inertactable note
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private LayerMask interactableLayer;

    // Interaction Inputs
    private InputAction interactKeyAction;
    private InputAction interactMouseAction;
    private bool tryInteractKey => interactKeyAction.triggered;
    private bool tryInteractMouse => interactMouseAction.triggered;

    // Interactable references
    Dictionary<IInteractable, GameObject> interactablesInRange;
    private IInteractable closestInteractable;
    private IInteractable mouseHoverInteractable;

    private Camera mainCamera;

    private void Start()
    {
        interactablesInRange = new Dictionary<IInteractable, GameObject>();
        interactKeyAction = GetComponent<PlayerInput>().actions["Interact"];
        interactMouseAction = GetComponent<PlayerInput>().actions["InteractMouse"];
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckForMouseOverInteractables();

        HandleClosestInteractable();

        HandleTryToInteract();
    }

    private void HandleTryToInteract()
    {
        // TODO: Handle the interaction depending on the input
    }

    private void HandleClosestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            interactCanvas.SetActive(false);
            if (mouseHoverInteractable == null || mouseHoverInteractable != closestInteractable)
            {
                closestInteractable?.RemoveInteractableOutline();
            }
            closestInteractable = null;

            return;
        }

        closestInteractable = GetClosestInteractable();

        if (closestInteractable != null && !closestInteractable.IsOutlineActive && !closestInteractable.IsOutlineActive)
        {
            closestInteractable.OutlineInteractable();
        }
    }

    private void CheckForMouseOverInteractables()
    {
        // Convert mouse position to world position
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // When using mouse to interact we what to ignore the triggers that are used for close player interaction
        Physics2D.queriesHitTriggers = false;
        // Cast a ray from the mouse position with the interaction layer mask
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, interactableLayer);
        Physics2D.queriesHitTriggers = true;

        if (hit.collider != null)
        {
            if (mouseHoverInteractable != null && mouseHoverInteractable != hit.collider.GetComponent<IInteractable>())
            {
                mouseHoverInteractable.RemoveInteractableOutline();
            }

            mouseHoverInteractable = hit.collider.GetComponent<IInteractable>();
            if (!mouseHoverInteractable.IsOutlineActive)
            {
                mouseHoverInteractable.OutlineInteractable();
            }
        }
        else
        {
            if (closestInteractable == null || closestInteractable != mouseHoverInteractable)
            {
                mouseHoverInteractable?.RemoveInteractableOutline();
                mouseHoverInteractable = null;
            }
        }
    }

    /// <summary>
    /// This method should never be costly cause having multiple available interactables should be rare
    /// But I wanted to cover all cases
    /// </summary>
    /// <returns>The Closest Interactable to the Player</returns>
    private IInteractable GetClosestInteractable()
    {
        IInteractable retVal = null;
        float closestDistance = float.MaxValue;

        foreach (var pair in new Dictionary<IInteractable, GameObject>(interactablesInRange))
        {
            IInteractable interactable = pair.Key;
            GameObject interactableObj = pair.Value;

            // This might happen in cases where an object is destryed, cause Unity doesn't trigger the OnTriggerExit2D event
            if (interactable == null)
            {
                interactablesInRange.Remove(interactable);
                continue;
            }

            float currentDistance = Vector2.Distance(transform.position, interactableObj.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                retVal = interactable;
            }
        }

        return retVal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable temp))
        {
            interactablesInRange.Add(temp, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable temp) &&
            interactablesInRange.ContainsKey(temp))
        {
            interactablesInRange.Remove(temp);
        }
    }
}
