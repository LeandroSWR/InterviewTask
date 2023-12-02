using UnityEngine;

public class GoldBagInteractable : InteractableBase
{
    [SerializeField] private PlayerCoins playerCoins;

    public override void Interact()
    {
        playerCoins.AddCoins(8000);
        gameObject.SetActive(false);
    }
}
