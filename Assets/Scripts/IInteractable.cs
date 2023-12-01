using UnityEngine;

public interface IInteractable
{
    // Where the player needs to be to interact with this object
    public Vector3 InteractionLocation { get; } 

    public Vector3 InteractNoteLocation { get; }
    public string InteractNoteText { get; }

    public void Interact();
}
