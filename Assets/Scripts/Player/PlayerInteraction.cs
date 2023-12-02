using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // Handle displaying the inertactable note
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private LayerMask interactableLayer;

    // Interaction Inputs
    private InputAction interactKeyAction;
    private bool tryInteractKey => interactKeyAction.triggered;

    // Interactable references
    Dictionary<IInteractable, GameObject> interactablesInRange;
    private IInteractable playerCloseInteractable;
    private IInteractable mouseHoverInteractable;
    private IInteractable currentInteractable;

    private Camera mainCamera;
    private PlayerController playerController;

    private void Start()
    {
        interactablesInRange = new Dictionary<IInteractable, GameObject>();
        interactKeyAction = GetComponent<PlayerInput>().actions["Interact"];
        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;
        playerController.OnReachInteractable += () => ReachedInteractable();
        playerController.OnCancelMoveToInteractable += () => CanceledInteraction();
    }

    private void Update()
    {
        HandleClosestInteractable();

        HandleInteractMenu();
    }

    public void InteractableClicked()
    {
        if (mouseHoverInteractable != null)
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            EnableInteractMenu(position, mouseHoverInteractable);
        }
    }

    private void HandleInteractMenu()
    {
        if (tryInteractKey && playerCloseInteractable != null)
        {
            Vector3 position = interactablesInRange[playerCloseInteractable].transform.position;
            EnableInteractMenu(position, playerCloseInteractable);
        }
    }

    private void EnableInteractMenu(Vector3 canvasPos, IInteractable interactable)
    {
        interactCanvas.SetActive(true);
        canvasPos.z = -0.1f;// Make sure the world space canvas is in front of evry object
        interactCanvas.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        currentInteractable = interactable;

        // Update Button Text
        interactCanvas.GetComponentInChildren<TMP_Text>().text = interactable.InteractOption;

        // Update Button Functionality
        Button interactCanvasBtt = interactCanvas.GetComponentInChildren<Button>();
        interactCanvasBtt.Select();
        interactCanvasBtt.onClick.RemoveAllListeners();
        interactCanvasBtt.onClick.AddListener(() => playerController.SetClickToMovePosition(interactable.InteractionLocation, true));
    }

    private void CanceledInteraction()
    {
        currentInteractable = null;
    }

    private void ReachedInteractable()
    {
        currentInteractable.Interact();
        currentInteractable = null;
    }

    private void HandleClosestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            if (mouseHoverInteractable == null && interactCanvas.activeSelf)
            {
                interactCanvas.SetActive(false);
            }

            if (mouseHoverInteractable == null || mouseHoverInteractable != playerCloseInteractable)
            {
                playerCloseInteractable?.RemoveInteractableOutline();
            }
            playerCloseInteractable = null;

            return;
        }

        playerCloseInteractable = GetClosestInteractable();

        if (playerCloseInteractable != null && !playerCloseInteractable.IsOutlineActive && !playerCloseInteractable.IsOutlineActive)
        {
            playerCloseInteractable.OutlineInteractable();
        }
    }

    public void MouseOverInteractable(IInteractable interactable)
    {
        if (mouseHoverInteractable != null && mouseHoverInteractable != interactable)
        {
            mouseHoverInteractable.RemoveInteractableOutline();
        }

        mouseHoverInteractable = interactable;
        if (!mouseHoverInteractable.IsOutlineActive)
        {
            mouseHoverInteractable.OutlineInteractable();
        }
    }

    public void MouseExitInteractable()
    {
        if (playerCloseInteractable == null || playerCloseInteractable != mouseHoverInteractable)
        {
            mouseHoverInteractable?.RemoveInteractableOutline();
            mouseHoverInteractable = null;
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
