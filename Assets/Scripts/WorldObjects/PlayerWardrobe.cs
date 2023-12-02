using System.Collections.Generic;
using UnityEngine;

public class PlayerWardrobe : ClothesManipulatorBase
{
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private ShopClothes shop;

    [Header("Player Clothes Sprites")]
    [SerializeField] private SpriteRenderer hoodSprite;
    [SerializeField] private SpriteRenderer torsoSprite;
    [SerializeField] private SpriteRenderer pelvisSprite;
    [SerializeField] private SpriteRenderer[] ShoulderSprites; // 0 = left, 1 = right
    [SerializeField] private SpriteRenderer[] BootSprites; // 0 = left, 1 = right
    [SerializeField] private SpriteRenderer[] GloveSprites; // 0 = elbow_L, 1 = wrist_L, 2 = elbow_R, 3 = wrist_R

    private List<ClothingPiece> dressedClothes;
    public List<ClothingPiece> DressedClothes => dressedClothes;

    private Dictionary<ClothingType, SpriteRenderer[]> clothingTypeToSprites;

    protected override void Start()
    {
        base.Start();

        isShop = false;

        print("PlayerWardrobe Start");
        dressedClothes = new List<ClothingPiece>(availableClothes);

        clothingTypeToSprites = new Dictionary<ClothingType, SpriteRenderer[]>
        {
            { ClothingType.Hood, new[] { hoodSprite } },
            { ClothingType.Torso, new[] { torsoSprite } },
            { ClothingType.Pelvis, new[] { pelvisSprite } },
            { ClothingType.Shoulder, ShoulderSprites },
            { ClothingType.Glove, GloveSprites },
            { ClothingType.Boot, BootSprites }
        };
    }

    public void SaveDressedClothes(List<ClothingPiece> dressedClothes)
    {
        this.dressedClothes = dressedClothes;

        foreach (ClothingPiece clothingPiece in dressedClothes)
        {
            UpdateClothes(clothingPiece);
        }
    }

    private void UpdateClothes(ClothingPiece clothingPiece)
    {
        if (clothingTypeToSprites.TryGetValue(clothingPiece.ClothingType, out SpriteRenderer[] images))
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (i < clothingPiece.DressableSprites.Length)
                {
                    images[i].sprite = clothingPiece.DressableSprites[i];
                }
            }
        }
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
