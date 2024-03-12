using UnityEngine;

public class ItemMechanic : MonoBehaviour
{
    protected HeldItem heldItemScript;
    public Item item;

    protected virtual void Awake()
    {
        heldItemScript = transform.parent.GetComponentInParent<HeldItem>();
    }
}
