using UnityEngine;

public class ShopClothes : ClothesManipulatorBase
{
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private PlayerWardrobe playerWardrobe;

    protected override void Start()
    {
        base.Start();

        isShop = true;
    }

    public override void FinalizeBusiness()
    {
        if (!playerCoins.TrySpendCoins(basketTotal))
        {
            return;
        }

        basketTotal = 0;
        for (int i = clothesInBasket.Count - 1; i >= 0; i--)
        {
            playerWardrobe.AddClothing(clothesInBasket[i]);
            clothesInBasket.Remove(clothesInBasket[i]);
        }
    }
}
