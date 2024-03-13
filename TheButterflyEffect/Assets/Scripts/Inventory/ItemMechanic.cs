using UnityEngine;

public class ItemMechanic : MonoBehaviour
{
    protected HeldItem heldItemScript;
    protected Animator animator;
    protected CharacterController controller;
    public Item item;

    protected virtual void Awake()
    {
        heldItemScript = transform.parent.GetComponentInParent<HeldItem>();
        animator = GetComponent<Animator>();
        controller = Inventory.Instance().GetComponent<CharacterController>();
    }
}
