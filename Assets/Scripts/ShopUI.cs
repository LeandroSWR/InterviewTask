using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject clothesShopParent;
    [SerializeField] private GameObject clothesBasketParent;
    [SerializeField] private TMP_Text basketTotalText;
    [SerializeField] private TMP_Text basketBalanceText;
    [SerializeField] private Button basketPurchaseBtt;
    [SerializeField] private GameObject clothesPrefab;
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private ShopClothes shop;
    [SerializeField] private MouseHandler mouseHandler;
    [SerializeField] private PlayerController playerController;

    private void OnEnable()
    {
        basketBalanceText.text = playerCoins.CurrentCoins.ToString();
        UpdateTotal();
    }
    
    public void FillShop()
    {
           // Clear the shop
        foreach (Transform child in clothesShopParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in clothesBasketParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Fill the shop
        foreach (ClothingPiece clothingPiece in shop.ClothesInShop)
        {
            // Create a temporary copy of the clothingPiece variable
            ClothingPiece tempClothing = clothingPiece;

            GameObject clothes = Instantiate(clothesPrefab, clothesShopParent.transform);
            clothes.GetComponent<ClothingPieceUI>().InitializeClothes(tempClothing);
            Button button = clothes.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                shop.MoveToBasket(tempClothing);
                UpdateTotal();

                // Disable the button
                button.interactable = false;
                button.transform.GetChild(2).gameObject.SetActive(true);

                // Spawn a copy of the clothes in the basket
                GameObject basketClothes = Instantiate(clothesPrefab, clothesBasketParent.transform);
                basketClothes.GetComponent<ClothingPieceUI>().InitializeClothes(tempClothing);

                // Add a listener to the basket clothes
                Button basketButton = basketClothes.GetComponent<Button>();
                basketButton.onClick.AddListener(() =>
                {
                    shop.RemoveFromBasket(tempClothing);
                    UpdateTotal();

                    // Enable the shop button
                    button.interactable = true;
                    button.transform.GetChild(2).gameObject.SetActive(false);

                    // Destroy the basket clothes
                    Destroy(basketClothes);
                });
            });
        }
    }

    private void UpdateTotal()
    {
        basketTotalText.text = shop.BasketTotal.ToString();

        if (shop.BasketTotal == 0 || shop.BasketTotal > playerCoins.CurrentCoins)
        {
            basketPurchaseBtt.interactable = false;
        }
        else
        {
            basketPurchaseBtt.interactable = true;
        }
    }

    public void CloseShop()
    {
        playerController.enabled = true;
        mouseHandler.enabled = true;
        shop.EmptyBasket();
        gameObject.SetActive(false);
    }
}