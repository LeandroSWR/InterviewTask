using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingPieceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text Price;
    [SerializeField] private Image clothesImage;

    public void InitializeClothes(ClothingPiece piece)
    {
        Price.text = piece.PurchasePrice.ToString();
        clothesImage.sprite = piece.DisplaySprite;
    }

    public void InitializeBasketClothes(ClothingPiece piece)
    {
        Price.text = piece.SellPrice.ToString();
        clothesImage.sprite = piece.DisplaySprite;
    }
}
