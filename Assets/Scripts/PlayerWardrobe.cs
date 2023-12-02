using System.Collections.Generic;
using UnityEngine;

public class PlayerWardrobe : MonoBehaviour
{
    [SerializeField] private List<ClothingPiece> clothingPieces;

    public void AddClothing(ClothingPiece clothingPiece)
    {           
        clothingPieces.Add(clothingPiece);
    }

    public void RemoveClothing(ClothingPiece clothingPiece)
    {
        if (!clothingPiece.Purchasable)
        {
            return;
        }

        clothingPieces.Remove(clothingPiece);
    }
}
