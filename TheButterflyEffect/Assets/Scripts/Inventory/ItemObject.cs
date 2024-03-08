using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;

    public void AddItemToInventory()
    {
        Inventory.Instance().AddItem(item);
        Destroy(gameObject);
    }
}