
using UnityEngine;
using UnityEngine.InputSystem;

public class WardrobeInteractable : InteractableBase
{
    [SerializeField] private WardrobeUI wardrobeUI;
    [SerializeField] private PlayerInput playerInput;

    public override void Interact()
    {
        playerInput.enabled = false;
        wardrobeUI.gameObject.SetActive(true);
        wardrobeUI.FillWardrobe();
    }
}
