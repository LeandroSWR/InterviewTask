using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ClothesManipulatorBase : MonoBehaviour
{
    [SerializeField] protected List<ClothingPiece> availableClothes;
    public List<ClothingPiece> AvailableClothes => availableClothes;

    protected List<ClothingPiece> clothesInBasket;
    protected int basketTotal = 0;
    public int BasketTotal => basketTotal;

    protected bool isShop;

    protected virtual void Start()
    {
        clothesInBasket = new List<ClothingPiece>();
        availableClothes = availableClothes.OrderBy(c => c.ClothesName).ToList();
    }

    public void MoveToBasket(ClothingPiece clothingPiece)
    {
        basketTotal += isShop ? clothingPiece.PurchasePrice : clothingPiece.SellPrice;
        clothesInBasket.Add(clothingPiece);
        availableClothes.Remove(clothingPiece);
    }

    public void RemoveFromBasket(ClothingPiece clothingPiece)
    {
        basketTotal -= isShop ? clothingPiece.PurchasePrice : clothingPiece.SellPrice;
        clothesInBasket.Remove(clothingPiece);
        availableClothes.Add(clothingPiece);
    }

    public void EmptyBasket()
    {
        basketTotal = 0;

        for (int i = clothesInBasket.Count - 1; i >= 0; i--)
        {
            availableClothes.Add(clothesInBasket[i]);
            clothesInBasket.Remove(clothesInBasket[i]);
        }

        availableClothes = availableClothes.OrderBy(c => c.ClothesName).ToList();
    }

    public void AddClothing(ClothingPiece clothingPiece)
    {
        if (!clothingPiece.Purchasable)
        {
            return;
        }

        availableClothes.Add(clothingPiece);
    }

    public abstract void FinalizeBusiness();
}
