using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
    // This makes it so that each interactable can have it's own text and text location
    [SerializeField] private string interactNoteText;
    public string InteractNoteText => interactNoteText;
    [SerializeField] private Transform interactNoteLocation;
    public Vector3 InteractNoteLocation => interactNoteLocation.position;

    [SerializeField] private Transform interactionLocation;
    public Vector3 InteractionLocation => interactionLocation.position;

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        Destroy(gameObject);
    }
}
