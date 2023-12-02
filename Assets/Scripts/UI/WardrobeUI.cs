using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WardrobeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject clothesParent;
    [SerializeField] private Button saveDressed;

    [Header("Visual Clothes Sprites")]
    [SerializeField] private Image hoodSprite;
    [SerializeField] private Image torsoSprite;
    [SerializeField] private Image pelvisSprite;
    [SerializeField] private Image[] ShoulderSprites; // 0 = left, 1 = right
    [SerializeField] private Image[] BootSprites; // 0 = left, 1 = right
    [SerializeField] private Image[] GloveSprites; // 0 = elbow_L, 1 = wrist_L, 2 = elbow_R, 3 = wrist_R

    [Header("References")]
    [SerializeField] private GameObject clothesPrefab;
    [SerializeField] private PlayerWardrobe playerWardrobe;
    [SerializeField] private PlayerInput playerInput;

    private List<ClothingPiece> dressedClothes;
    private Dictionary<ClothingPiece, Button> clothesButtons;
    private Dictionary<ClothingType, Image[]> clothingTypeToSprites;

    private void Awake()
    {
        dressedClothes = new List<ClothingPiece>(playerWardrobe.DressedClothes);

        saveDressed.onClick.AddListener(() =>
        {
            playerWardrobe.SaveDressedClothes(dressedClothes);
            CloseWardrobe();
        });

        clothesButtons = new Dictionary<ClothingPiece, Button>();
        clothingTypeToSprites = new Dictionary<ClothingType, Image[]>
        {
            { ClothingType.Hood, new[] { hoodSprite } },
            { ClothingType.Torso, new[] { torsoSprite } },
            { ClothingType.Pelvis, new[] { pelvisSprite } },
            { ClothingType.Shoulder, ShoulderSprites },
            { ClothingType.Glove, GloveSprites },
            { ClothingType.Boot, BootSprites }
        };
    }

    public void FillWardrobe()
    {
        dressedClothes = new List<ClothingPiece>(playerWardrobe.DressedClothes);

        // Clear the sell
        foreach (Transform child in clothesParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Fill the shop
        foreach (ClothingPiece clothingPiece in playerWardrobe.AvailableClothes)
        {
            // Create a temporary copy of the clothingPiece variable
            ClothingPiece tempClothing = clothingPiece;

            GameObject clothes = Instantiate(clothesPrefab, clothesParent.transform);
            clothes.GetComponent<ClothingPieceUI>().InitializeWardrobeClothes(tempClothing);
            Button button = clothes.GetComponent<Button>();

            // Disable the button if the player already has the clothing piece
            if (dressedClothes.Contains(tempClothing))
            {
                button.interactable = false;
                button.transform.GetChild(2).gameObject.SetActive(true);
            }

            button.onClick.AddListener(() =>
            {
                UpdateEquippedClothes(tempClothing);

                // Disable the button
                button.interactable = false;
                button.transform.GetChild(2).gameObject.SetActive(true);
            });

            clothesButtons[tempClothing] = button;
        }
    }

    private void UpdateEquippedClothes(ClothingPiece clothingPiece)
    {
        // Update the visual sprites
        if (clothingTypeToSprites.TryGetValue(clothingPiece.ClothingType, out Image[] images))
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (i < clothingPiece.DressableSprites.Length)
                {
                    images[i].sprite = clothingPiece.DressableSprites[i];
                }
            }
        }

        // Update the internal list
        if (dressedClothes.Contains(clothingPiece))
        {
            return;
        }

        for (int i = 0; i < dressedClothes.Count; i++)
        {
            if (dressedClothes[i].ClothingType == clothingPiece.ClothingType)
            {
                // Update the button
                clothesButtons[dressedClothes[i]].interactable = true;
                clothesButtons[dressedClothes[i]].transform.GetChild(2).gameObject.SetActive(false);

                dressedClothes[i] = clothingPiece;
                return;
            }
        }

        dressedClothes.Add(clothingPiece);
    }

    public void CloseWardrobe()
    {
        dressedClothes = new List<ClothingPiece>(playerWardrobe.DressedClothes);
        foreach (ClothingPiece clothingPiece in dressedClothes)
        {
            UpdateEquippedClothes(clothingPiece);
        }

        playerInput.enabled = true;
        gameObject.SetActive(false);
    }
}
