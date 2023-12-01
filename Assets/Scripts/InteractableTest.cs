using UnityEngine;

public class InteractableTest : InteractableBase
{
    public override void Interact(string a)
    {
        Debug.Log("Interacted with " + gameObject.name);
        Destroy(gameObject);
    }
}
