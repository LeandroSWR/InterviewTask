using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Interaction Inputs
    private InputAction interactAction;
    private bool tryInteract => interactAction.triggered;

    // Interactable references
    Dictionary<IInteractable, GameObject> interactablesInRange;
    private IInteractable closestInteractable;

    private void Start()
    {
        interactablesInRange = new Dictionary<IInteractable, GameObject>();
        interactAction = GetComponent<PlayerInput>().actions["Interact"];
    }

    private void Update()
    {
        if (interactablesInRange.Count == 0)
        {
            closestInteractable = null;
            return;
        }

        closestInteractable = GetClosestInteractable();

        if (tryInteract && closestInteractable != null)
        {
            closestInteractable.Interact();
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

        float prevDistance = float.MaxValue;
        float currentDistance;
        foreach (IInteractable interactable in interactablesInRange.Keys)
        {
            // This might happen in cases where an object is destryed, cause Unity doesn't trigger the OnTriggerExit2D event
            if (interactable == null)
            {
                interactablesInRange.Remove(interactable);
                continue;
            }

            currentDistance = Vector2.Distance(transform.position, interactablesInRange[interactable].transform.position);
            if (currentDistance < prevDistance)
            {
                prevDistance = currentDistance;
                retVal = interactable;
            }
        }

        return retVal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with " + collision.gameObject.name);
        if (collision.TryGetComponent(out IInteractable temp))
        {
            Debug.Log("Added " + collision.gameObject.name + " to interactablesInRange");
            interactablesInRange.Add(temp, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable temp) &&
            interactablesInRange.ContainsKey(temp))
        {
            Debug.Log("Removed " + collision.gameObject.name + " from interactablesInRange");
            interactablesInRange.Remove(temp);
        }
    }
}
