using System.Collections.Generic;
using UnityEngine;

public class PlayerWardrobe : ClothesManipulatorBase
{
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private ShopClothes shop;

    private List<ClothingPiece> dressedClothes;
    
    protected override void Start()
    {
        base.Start();

        isShop = false;

        dressedClothes = availableClothes;
    }

    public override void FinalizeBusiness()
    {
        playerCoins.AddCoins(basketTotal);

        basketTotal = 0;
        for (int i = clothesInBasket.Count - 1; i >= 0; i--)
        {
            shop.AddClothing(clothesInBasket[i]);
            clothesInBasket.Remove(clothesInBasket[i]);
        }
    }
}
