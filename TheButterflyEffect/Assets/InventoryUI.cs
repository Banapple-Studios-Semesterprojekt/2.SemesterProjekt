using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlot[] slots;
    //[SerializeField] private GameObject grabbedObjPrefab;

    private PointerEventData pointerData;
    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;

    private InventoryItem grabbedItem;
    private GameObject grabbedItemGO;
    private int grabbedItemIndex;

    private GameObject inventorySlotsParent;

    private void Start()
    {
        Inventory.Instance().onAddItem += AddItemEvent;
        Inventory.Instance().onToggleInventory += ToggleInventoryEvent;
        inventorySlotsParent = transform.Find("Inventory UI").gameObject;
        slots = inventorySlotsParent.transform.Find("Inventory Slots").GetComponentsInChildren<InventorySlot>();
        eventSystem = FindAnyObjectByType<EventSystem>();
        raycaster = transform.GetComponent<GraphicRaycaster>();
        ClearAllSlots();
        inventorySlotsParent.SetActive(false);
    }

    private void ToggleInventoryEvent(bool isActive)
    {
        if(isActive)
        {
            PlayerController.playerInput.Player.PrimaryAction.performed += InventorySlotInteract;
        }
        else
        {
            PlayerController.playerInput.Player.PrimaryAction.performed -= InventorySlotInteract;
        }
    }

    private void Update()
    {
        if(grabbedItemGO != null)
        {
            grabbedItemGO.transform.position = Mouse.current.position.value;
        }
    }

    private void InventorySlotInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Click");
        pointerData = new PointerEventData(eventSystem);
        pointerData.position = Mouse.current.position.value;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        if (results.Count == 0) 
        { 
            if(grabbedItemGO == null) 
            { 
                return; 
            }
            else if(grabbedItemGO != null)
            {
                Inventory.Instance().RemoveItem(grabbedItemIndex);
                StartCoroutine(DropItem(grabbedItem));
                grabbedItem = null;
                Destroy(grabbedItemGO);
            }
            return; 
        }
        
        InventorySlot hitSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
        grabbedItemIndex = Array.IndexOf(slots, hitSlot);
        Debug.Log(grabbedItemIndex);
        if (grabbedItemGO != null && hitSlot != null && hitSlot.currentItem == null)
        {
            Inventory.Instance().UpdateItem(grabbedItem, grabbedItemIndex);
            slots[grabbedItemIndex].SetInventorySlot(grabbedItem);
            grabbedItem = null;
            Destroy(grabbedItemGO);
            grabbedItemGO = null;
            return;
        }
        else if(hitSlot != null && hitSlot.currentItem != null)
        {
            grabbedItem = hitSlot.currentItem;
            grabbedItemGO = new GameObject("Grabbed Item Icon");
            grabbedItemGO.transform.SetParent(transform);
            grabbedItemGO.name = "Grabbed Item Icon";
            Image image = grabbedItemGO.AddComponent<Image>();
            image.sprite = hitSlot.currentItem.item.itemSprite;
            image.raycastTarget = false;
            slots[grabbedItemIndex].SetInventorySlot(null);
            Inventory.Instance().RemoveItem(grabbedItemIndex);
            print("Instanitate");
        }
    }

    private void AddItemEvent(InventoryItem item, int index)
    {
        SetSpecificSlot(item, index);
    }

    public void ClearAllSlots()
    {
        foreach (InventorySlot item in slots)
        {
            item.SetInventorySlot(null);
        }
    }

    public void SetSpecificSlot(InventoryItem item, int index)
    {
        slots[index].SetInventorySlot(item);
    }

    IEnumerator DropItem(InventoryItem item)
    {
        int stack = item.currentStack;
        for (int i = 0; i < stack; i++)
        {
            GameObject droppedItem = Instantiate(item.item.itemObject, Inventory.Instance().transform.position, Quaternion.identity);
            droppedItem.name = item.item.name;
            Debug.Log("SPAWNING ITEM!");
            yield return new WaitForSeconds(0.2f);
        }
    }

    void DropItemInstantly(InventoryItem item)
    {
        for (int i = 0; i < item.currentStack; i++)
        {
            GameObject droppedItem = Instantiate(item.item.itemObject, Inventory.Instance().transform.position, Quaternion.identity);
            droppedItem.name = item.item.name;
            Debug.Log("SPAWNING ITEM!");
        }
    }
}