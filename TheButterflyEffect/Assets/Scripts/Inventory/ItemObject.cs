using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    [SerializeField] private bool canTake = true;

    public void AddItemToInventory()
    {
        if(!canTake) { return; }

        Inventory.Instance().AddItem(item);
        Destroy(gameObject);
    }

    public void AddItemToInventoryWithNet()
    {
        Inventory.Instance().AddItem(item);
        Destroy(gameObject);
    }
}
