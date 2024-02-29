using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private int inventoryCapacity = 10;
    [SerializeField] private GameObject inventoryCanvas;

    private PlayerController playerController;

    public delegate void AddItemAction(InventoryItem item, int index);
    public event AddItemAction onAddItem;
    public delegate void ToggleInventoryAction(bool isActive);
    public event ToggleInventoryAction onToggleInventory;

    private void Start()
    {
        inventoryItems = new InventoryItem[inventoryCapacity];
        PlayerController.playerInput.Player.Inventory.performed += ToggleInventory;
        playerController = GetComponent<PlayerController>();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        playerController.enabled = !playerController.enabled;
        inventoryCanvas.SetActive(!playerController.enabled);
        GetComponent<Interactor>().enabled = playerController.enabled;
        Cursor.visible = !playerController.enabled;
        Cursor.lockState = !playerController.enabled ? CursorLockMode.None : CursorLockMode.Locked;

        onToggleInventory?.Invoke(!playerController.enabled);
    }

    public bool AddItem(Item newItem)
    {
        /*
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if(inventoryItem.item == newItem)
            {
                if(inventoryItem.currentStack >= inventoryItem.item.maxStack) { continue; }
                inventoryItem.currentStack++;
                onAddItem?.Invoke(inventoryItem, Array.IndexOf(inventoryItems, inventoryItem));
                return true;
            }
        }*/
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] != null && inventoryItems[i].item == newItem)
            {
                if (inventoryItems[i].currentStack >= inventoryItems[i].item.maxStack) { continue; }
                inventoryItems[i].currentStack++;
                onAddItem?.Invoke(inventoryItems[i], Array.IndexOf(inventoryItems, inventoryItems[i]));
                return true;
            }
        }

        int index;
        for (index = 0; index < inventoryItems.Length - 1; index++)
        {
            if(inventoryItems[index] == null)
            {
                break;
            }
        }
        Debug.Log(index);
        InventoryItem invItem = new InventoryItem(newItem);
        inventoryItems[index] = invItem;

        onAddItem?.Invoke(invItem, Array.IndexOf(inventoryItems, invItem));

        return true;
    }

    public void UpdateItem(InventoryItem item, int index)
    {
        if(inventoryItems[index] != item)
        {
            inventoryItems[index] = item;
            return;
        }
        InventoryItem storedItem = inventoryItems[index];
        if(storedItem.item == item.item)
        {
            if((storedItem.currentStack + item.currentStack) <= item.item.maxStack)
            {
                storedItem.currentStack += item.currentStack;
            }
            else
            {
                storedItem.currentStack += item.currentStack;
                int exchange = storedItem.currentStack - storedItem.item.maxStack;
                item.currentStack = exchange;
                storedItem.currentStack = storedItem.item.maxStack;
            }
        }
    }

    public void RemoveItem(int index)
    {
        inventoryItems[index] = null;
    }
}
