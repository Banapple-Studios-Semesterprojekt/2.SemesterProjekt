using UnityEngine;

public class SwingNet : ItemMechanic
{   
    public delegate void SwingAction();
    public event SwingAction onSwing;

    private void OnEnable()
    {
        heldItemScript.onPrimaryAction += Swing;
    }

    private void OnDisable()
    {
        heldItemScript.onPrimaryAction -= Swing;
    }

    private void Swing()
    {
        onSwing?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ItemObject>() && other.GetComponent<ItemObject>().item.itemType == ItemType.Butterfly)
        {
            other.GetComponent<ItemObject>().AddItemToInventoryWithNet();
        }
    }
}
