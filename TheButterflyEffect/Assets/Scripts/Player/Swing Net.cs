using UnityEngine;
using UnityEngine.InputSystem;


public class SwingNet : ItemMechanic
{
    private const int maxSpeed = 6;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        heldItemScript.onPrimaryAction += Swing;
    }

    private void OnDisable()
    {
        heldItemScript.onPrimaryAction -= Swing;
    }

    private void Update()
    {
        animator.SetFloat("Velocity", controller.velocity.magnitude / maxSpeed, 0.125f, Time.deltaTime);
    }
    private void Swing()
    {
        animator.SetTrigger("Swing");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ItemObject>() && other.GetComponent<ItemObject>().item.itemType == ItemType.Butterfly)
        {
            other.GetComponent<ItemObject>().AddItemToInventoryWithNet();
        }
    }
}
