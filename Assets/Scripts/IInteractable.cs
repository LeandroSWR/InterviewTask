using UnityEngine;

public interface IInteractable
{
    // The options the player has to interact with this object
    public string InteractOption { get; }
    // Where the player needs to be to interact with this object
    public Vector3 InteractionLocation { get; } 
    public bool IsOutlineActive { get; }

    public void Interact();
    public void OutlineInteractable();
    public void RemoveInteractableOutline();
}
