using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingPieceUI : MonoBehaviour
{
    [SerializeField] private GameObject priceObj;
    [SerializeField] private TMP_Text Price;
    [SerializeField] private Image clothesImage;

    public void InitializeClothes(ClothingPiece piece, bool isSell = false)
    {
        Price.text = isSell ? piece.SellPrice.ToString() : piece.PurchasePrice.ToString();
        clothesImage.sprite = piece.DisplaySprite;
    }

    public void InitializeWardrobeClothes(ClothingPiece piece)
    {
        // Don't display a prince if the clothes are in the wardrobe
        priceObj.SetActive(false);
        clothesImage.sprite = piece.DisplaySprite;
    }
}
