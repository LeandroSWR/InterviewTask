using UnityEngine;

public class InteractableTest : InteractableBase
{
    public override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
