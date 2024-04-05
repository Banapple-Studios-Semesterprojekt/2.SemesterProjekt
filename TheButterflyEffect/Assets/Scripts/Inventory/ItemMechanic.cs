using UnityEngine;

public class ItemMechanic : MonoBehaviour
{
    protected HeldItem heldItemScript;
    protected CharacterController controller;
    public Item item;

    protected virtual void Awake()
    {
        heldItemScript = transform.parent.GetComponentInParent<HeldItem>();
        controller = Inventory.Instance().GetComponent<CharacterController>();
    }
}
