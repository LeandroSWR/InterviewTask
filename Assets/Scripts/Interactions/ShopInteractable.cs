using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInteractable : InteractableBase
{
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private PlayerInput playerInput;

    public override void Interact()
    {
        // Open Store UI and fill in the clothing pieces
        playerInput.enabled = false;
        shopUI.gameObject.SetActive(true);
        shopUI.FillShop();
    }
}
