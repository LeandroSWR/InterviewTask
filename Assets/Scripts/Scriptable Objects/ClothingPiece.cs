using UnityEngine;

[CreateAssetMenu(fileName = "Clothes", menuName = "ClothingPiece", order = 1)]
public class ClothingPiece : ScriptableObject
{
    // The name of the clothes
    [SerializeField] private string clothesName;
    public string ClothesName => clothesName;

    // The sprite that's displayed in the UI
    [SerializeField] private Sprite displaySprite;
    public Sprite DisplaySprite => displaySprite;

    // The sprites that are used to dress the character
    [SerializeField] private Sprite[] dressableSprites;
    public Sprite[] DressableSprites => dressableSprites;

    // The type of clothing piece
    [SerializeField] private ClothingType clothingType;
    public ClothingType ClothingType => clothingType;

    // If this clothes can be purchased/sold
    [SerializeField] bool purchasable;
    public bool Purchasable => purchasable;

    // The price of the clothes
    [SerializeField] int purchasePrice;
    public int PurchasePrice => purchasePrice;

    // The price of the clothes when sold
    [SerializeField] int sellPrice;
    public int SellPrice => sellPrice;
}
