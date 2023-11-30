using UnityEngine;

public interface IInteractable
{
    public Vector3 InteractNoteLocation { get; }
    public string InteractNoteText { get; }

    public void Interact();
}
