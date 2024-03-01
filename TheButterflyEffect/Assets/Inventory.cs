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
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i] = new InventoryItem(null, 0);
        }
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

        //This for-loop makes NO sense... It works fine when the inspector isn't looking at the array, but not when it does...
        int index;
        for (index = 0; index < inventoryItems.Length - 1; index++)
        {
            if(inventoryItems[index].item == null)
            {
                break;
            }
        }
        if(index == inventoryCapacity - 1 && inventoryItems[inventoryCapacity - 1].item != null) { return false; }
        InventoryItem invItem = new InventoryItem(newItem);
        inventoryItems[index] = invItem;

        onAddItem?.Invoke(invItem, Array.IndexOf(inventoryItems, invItem));

        return true;
    }

    public InventoryItem UpdateItem(InventoryItem item, int index)
    {
        if(inventoryItems[index] != null && inventoryItems[index].item != null && inventoryItems[index].item == item.item)
        {
            if((inventoryItems[index].currentStack + item.currentStack) <= item.item.maxStack)
            {
                inventoryItems[index].currentStack += item.currentStack;
            }
            else
            {
                inventoryItems[index].currentStack += item.currentStack;
                int exchange = inventoryItems[index].currentStack - inventoryItems[index].item.maxStack;
                item.currentStack = exchange;
                inventoryItems[index].currentStack = inventoryItems[index].item.maxStack;
            }
        }
        else
        {
            inventoryItems[index].item = item.item;
            inventoryItems[index].currentStack = item.currentStack;
            //inventoryItems[index] = item;
            Debug.Log(item.item.itemType);
        }
        return inventoryItems[index];
    }

    public void RemoveItem(int index)
    {
        inventoryItems[index].item = null;
        inventoryItems[index].currentStack = 0;
    }

}
