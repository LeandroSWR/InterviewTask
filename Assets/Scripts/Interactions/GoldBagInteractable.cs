using UnityEngine;

public class GoldBagInteractable : InteractableBase
{
    [SerializeField] private PlayerCoins playerCoins;

    public override void Interact()
    {
        playerCoins.AddCoins(5000);
        gameObject.SetActive(false);
    }
}
