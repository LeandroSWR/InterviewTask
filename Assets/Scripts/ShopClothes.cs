using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopClothes : MonoBehaviour
{
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private PlayerWardrobe playerWardrobe;
    [SerializeField] private List<ClothingPiece> clothesInShop;
    public List<ClothingPiece> ClothesInShop => clothesInShop;

    private List<ClothingPiece> clothesInBasket;
    private int basketTotal = 0;
    public int BasketTotal => basketTotal;

    private void Start()
    {
        clothesInBasket = new List<ClothingPiece>();
    }

    public void MoveToBasket(ClothingPiece clothingPiece)
    {
        basketTotal += clothingPiece.PurchasePrice;
        clothesInBasket.Add(clothingPiece);
        clothesInShop.Remove(clothingPiece);
    }

    public void RemoveFromBasket(ClothingPiece clothingPiece)
    {
        basketTotal -= clothingPiece.PurchasePrice;
        clothesInBasket.Remove(clothingPiece);
        clothesInShop.Add(clothingPiece);
    }

    public void EmptyBasket()
    {
        basketTotal = 0;

        for (int i = clothesInBasket.Count - 1; i >= 0; i--)
        {
            clothesInShop.Add(clothesInBasket[i]);
            clothesInBasket.Remove(clothesInBasket[i]);
        }

        clothesInShop = clothesInShop.OrderBy(c => c.ClothesName).ToList();
    }

    public void AddClothing(ClothingPiece clothingPiece)
    {
        if (!clothingPiece.Purchasable)
        {
            return;
        }

        playerCoins.AddCoins(clothingPiece.SellPrice);
        playerWardrobe.RemoveClothing(clothingPiece);

        clothesInShop.Add(clothingPiece);
    }

    public void MakePurchase()
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
