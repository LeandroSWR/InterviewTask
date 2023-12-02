using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private GameObject clothesShopParent;
    [SerializeField] private GameObject clothesSellParent;
    [SerializeField] private GameObject clothesBasketParent;
    [SerializeField] private GameObject clothesSellBasketParent;
    [SerializeField] private TMP_Text basketBuyTotalText;
    [SerializeField] private TMP_Text basketSellTotalText;
    [SerializeField] private TMP_Text basketBalanceText;
    [SerializeField] private Button basketPurchaseBtt;
    [SerializeField] private Button basketSellBtt;

    [Header("References")]
    [SerializeField] private GameObject clothesPrefab;
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private MouseHandler mouseHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ShopClothes shop;
    [SerializeField] private PlayerWardrobe playerWardrobe;

    private void OnEnable()
    {
        basketBalanceText.text = playerCoins.CurrentCoins.ToString();
        UpdateTotal(shop);
        UpdateTotal(playerWardrobe);
    }

    public void FillShop()
    {
        FillBuySell(shop);
        FillBuySell(playerWardrobe);
        sellPanel.SetActive(false);
        buyPanel.SetActive(true);
    }

    public void FillBuySell(ClothesManipulatorBase clothesManipulator)
    {
        if (clothesManipulator is ShopClothes)
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
        }
        else
        {
            // Clear the sell
            foreach (Transform child in clothesSellParent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in clothesSellBasketParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Fill the shop
        foreach (ClothingPiece clothingPiece in clothesManipulator.AvailableClothes)
        {
            if (!clothingPiece.Purchasable)
            {
                continue;
            }

            // Create a temporary copy of the clothingPiece variable
            ClothingPiece tempClothing = clothingPiece;

            Transform parent = clothesManipulator is ShopClothes ? clothesShopParent.transform : clothesSellParent.transform;
            GameObject clothes = Instantiate(clothesPrefab, parent);
            clothes.GetComponent<ClothingPieceUI>().InitializeClothes(tempClothing);
            Button button = clothes.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                clothesManipulator.MoveToBasket(tempClothing);
                UpdateTotal(clothesManipulator);

                // Disable the button
                button.interactable = false;
                button.transform.GetChild(2).gameObject.SetActive(true);

                // Spawn a copy of the clothes in the basket
                parent = clothesManipulator is ShopClothes ? clothesBasketParent.transform : clothesSellBasketParent.transform;
                GameObject basketClothes = Instantiate(clothesPrefab, parent);
                basketClothes.GetComponent<ClothingPieceUI>().InitializeClothes(tempClothing);

                // Add a listener to the basket clothes
                Button basketButton = basketClothes.GetComponent<Button>();
                basketButton.onClick.AddListener(() =>
                {
                    clothesManipulator.RemoveFromBasket(tempClothing);
                    UpdateTotal(clothesManipulator);

                    // Enable the shop button
                    button.interactable = true;
                    button.transform.GetChild(2).gameObject.SetActive(false);

                    // Destroy the basket clothes
                    Destroy(basketClothes);
                });
            });
        }
    }

    private void UpdateTotal(ClothesManipulatorBase clothesManipulator)
    {
        if (clothesManipulator is ShopClothes)
        {
            basketBuyTotalText.text = clothesManipulator.BasketTotal.ToString();
            basketSellTotalText.text = "0";
        }
        else
        {
            basketBuyTotalText.text = "0";
            basketSellTotalText.text = clothesManipulator.BasketTotal.ToString();
        }
        
        if (clothesManipulator.BasketTotal == 0 || clothesManipulator.BasketTotal > playerCoins.CurrentCoins)
        {
            if (clothesManipulator is ShopClothes)
            {
                basketPurchaseBtt.interactable = false;
            }
            else
            {
                basketSellBtt.interactable = false;
            }
        }
        else
        {
            if (clothesManipulator is ShopClothes)
            {
                basketPurchaseBtt.interactable = true;
            }
            else
            {
                basketSellBtt.interactable = true;
            }
        }
    }

    public void CloseShop()
    {
        playerController.enabled = true;
        mouseHandler.enabled = true;
        shop.EmptyBasket();
        playerWardrobe.EmptyBasket();
        gameObject.SetActive(false);
    }
}