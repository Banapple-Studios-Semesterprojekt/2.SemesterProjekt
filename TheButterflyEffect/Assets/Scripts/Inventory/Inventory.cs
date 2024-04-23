using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private int inventoryCapacity = 10;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject HotBarCanvas;
    [SerializeField] private GameObject buttons;

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
    private void OnDestroy()
    {
        PlayerController.playerInput.Player.Inventory.performed -= ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        playerController.enabled = !playerController.enabled;
        SetInventory(!playerController.enabled);
        buttons.SetActive(!playerController.enabled);
    }

    public void SetInventory(bool isActive)
    {
        print("Sat inventory ui to: " + isActive);
        playerController.enabled = !isActive;
        inventoryCanvas.SetActive(isActive);
        HotBarCanvas.SetActive(true);

        Cursor.visible = isActive;
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

        onToggleInventory?.Invoke(isActive);
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
        int index;
        for (index = 0; index < inventoryItems.Length - 1; index++)
        {
            if(inventoryItems[index].item == null)
            {
                break;
            }
        }
        if(index == inventoryCapacity - 1 && inventoryItems[inventoryCapacity - 1].item != null) { return false; }
        if(newItem.itemType == ItemType.Tools)
        {
            for (index = inventoryCapacity - 3; index < inventoryCapacity; index++)
            {
                if (inventoryItems[index].item == null)
                {
                    break;
                }
            }
        }
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
        }
        return inventoryItems[index];
    }

    public void RemoveItem(int index)
    {
        inventoryItems[index].item = null;
        inventoryItems[index].currentStack = 0;
    }

    public int getinventoryCapacity()
    {
        return inventoryCapacity;
    }

    public void isDead(bool d)
    {
        if (d)
        {
            PlayerController.playerInput.Player.Inventory.performed -= ToggleInventory;
            inventoryCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            PlayerController.playerInput.Player.Inventory.performed += ToggleInventory;
        }
    }

    public void Empty_inventory(bool dropinven, bool Empty_Hotbar)
    {
        int invensize;
        if (Empty_Hotbar)
        {
            invensize = 15;
        }
        else
        {
            invensize = 12;
        }
        InventoryUI invenUI = GetComponentInChildren<InventoryUI>();
        for (int i = 0; i < invensize; i++)
        {
            if (dropinven)
            {
                for (int j = 0; j < inventoryItems[i].currentStack; j++)
                {
                    GameObject droppedItem = Instantiate(inventoryItems[i].item.itemObject, transform.position + Vector3.up * 0.5f + transform.forward, Quaternion.identity);
                    droppedItem.name = inventoryItems[i].item.name;
                }
            }
            RemoveItem(i);
            if (!Empty_Hotbar)
            {
                invenUI.slots[i].RemoveInventorySlot();
            }
        }
        if (Empty_Hotbar)
        {
            invenUI.ClearAllSlots();
        }
    }

}
