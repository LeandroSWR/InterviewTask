using UnityEngine;

public class ShopInteractable : InteractableBase
{
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseHandler mouseHandler;

    public override void Interact()
    {
        // Open Store UI and fill in the clothing pieces
        playerController.enabled = false;
        mouseHandler.enabled = false;
        shopUI.gameObject.SetActive(true);
        shopUI.FillShop();
    }
}
