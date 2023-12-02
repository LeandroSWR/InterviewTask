using System.Collections;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer outlineSprite;
    [SerializeField] private Transform interactionLocation;
    public Vector3 InteractionLocation => interactionLocation.position;

    [SerializeField] public string interactOption;
    public string InteractOption => interactOption;
    
    private bool isOutlineActive;
    public bool IsOutlineActive => isOutlineActive;

    public abstract void Interact();

    public void OutlineInteractable()
    {
        StopAllCoroutines();
        isOutlineActive = true;
        StartCoroutine(UpdateOutline(0f, 1f));
    }

    public void RemoveInteractableOutline()
    {
        StopAllCoroutines();
        isOutlineActive = false;
        StartCoroutine(UpdateOutline(1f, 0f));
    }

    private IEnumerator UpdateOutline(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = outlineSprite.color;
        
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / 0.5f);
            color.a = alpha;
            outlineSprite.color = color;
            yield return null;
        }

        // Ensure the final alpha is set to endAlpha
        color.a = endAlpha;
        outlineSprite.color = color;
    }
}
