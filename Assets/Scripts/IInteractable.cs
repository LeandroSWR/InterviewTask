using UnityEngine;

public interface IInteractable
{
    // The options the player has to interact with this object
    public string[] InteractOptions { get; }
    // Where the player needs to be to interact with this object
    public Vector3 InteractionLocation { get; } 
    public bool IsOutlineActive { get; }

    public void Interact(string interactOption);
    public void OutlineInteractable();
    public void RemoveInteractableOutline();
}
