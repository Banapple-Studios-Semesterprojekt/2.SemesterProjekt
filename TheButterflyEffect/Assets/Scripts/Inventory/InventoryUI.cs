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

    //canvas references
    private PointerEventData pointerData;

    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;

    //grabbed item properties (the one your holding in the inventory)
    private InventoryItem grabbedItem;

    private GameObject grabbedItemGO;
    private int grabbedItemIndex;

    //the parent object for all inventory slots
    private GameObject inventorySlotsParent;

    private void Start()
    {
        //Subscribing events from Inventory instance
        Inventory.Instance().onAddItem += AddItemEvent;
        Inventory.Instance().onToggleInventory += ToggleInventoryEvent;

        //Finding the parent object of the inventory slots
        inventorySlotsParent = transform.Find("Inventory UI").gameObject;

        //Finding all the inventory slots through the parent object
        slots = inventorySlotsParent.transform.Find("Inventory Slots").GetComponentsInChildren<InventorySlot>();

        //Finding the event system and graphic raycaster for the canvas
        eventSystem = FindAnyObjectByType<EventSystem>();
        raycaster = transform.GetComponent<GraphicRaycaster>();

        //Resetting all slots (might be changed later for saving data purposes)
        ClearAllSlots();

        //Setting the UI inventory to invisible
        inventorySlotsParent.SetActive(false);
    }

    private void Update()
    {
        UpdateGrabbedItemToMouse();
    }

    #region Events

    private void AddItemEvent(InventoryItem item, int index)
    {
        SetSpecificSlot(item, index);
    }

    private void ToggleInventoryEvent(bool isActive)
    {
        if (isActive)
        {
            PlayerController.playerInput.Player.PrimaryAction.performed += InventorySlotInteract;
            PlayerController.playerInput.Player.SecondaryAction.performed += StackSplit;
        }
        else
        {
            PlayerController.playerInput.Player.PrimaryAction.performed -= InventorySlotInteract;
            PlayerController.playerInput.Player.SecondaryAction.performed -= StackSplit;
        }
    }

    #endregion Events

    #region UI_Interaction

    private void UpdateGrabbedItemToMouse()
    {
        //Making the grabbed item follow the mouse/cursor
        if (grabbedItemGO != null)
        {
            Vector2 mouseDelta = PlayerController.playerInput.Player.CameraLook.ReadValue<Vector2>();
            float xScale = 1 + Mathf.Abs(mouseDelta.x) * 0.7f; xScale = Mathf.Clamp(xScale, 1f, 3f);
            float yScale = 1 + Mathf.Abs(mouseDelta.y) * 0.7f; yScale = Mathf.Clamp(yScale, 1f, 3f);
            grabbedItemGO.transform.position = Mouse.current.position.value;
            grabbedItemGO.transform.localScale = Vector3.Lerp(grabbedItemGO.transform.localScale, new Vector2(xScale, yScale), 15 * Time.deltaTime);
        }
    }

    private void InventorySlotInteract(InputAction.CallbackContext context)
    {
        //First raycast to any ui graphic, to select an item from the inventory
        List<RaycastResult> results = UIRaycasting();

        //Secondly check if mouse is clicking outside the inventory window. If so, throw out item if is grabbed
        bool isGrabbedandDropping = DropItemIfGrabbed(results.ToArray());
        if (isGrabbedandDropping) { return; }

        //Retrieving the clicked inventory slot
        InventorySlot hitSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
        grabbedItemIndex = Array.IndexOf(slots, hitSlot);

        //If the clicked slot is occupied and no grabbed object, take the object on mouse, else if clicked empty slot with grabbed item, place it
        TakeOrPlaceItem(hitSlot, true);
    }

    private void StackSplit(InputAction.CallbackContext context)
    {
        if (grabbedItemGO != null)
        {
            List<RaycastResult> results = UIRaycasting();
            if (results.Count <= 0) { return; }
            InventorySlot hitSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
            grabbedItemIndex = Array.IndexOf(slots, hitSlot);
            if (hitSlot != null && hitSlot.currentItem != null && hitSlot.currentItem.item != grabbedItem.item) { return; }
            TakeOrPlaceItem(hitSlot, false);
        }
    }

    private List<RaycastResult> UIRaycasting()
    {
        pointerData = new PointerEventData(eventSystem);
        pointerData.position = Mouse.current.position.value;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        return results;
    }

    private bool DropItemIfGrabbed(RaycastResult[] results)
    {
        if (results.Length == 0)
        {
            if (grabbedItemGO == null)
            {
                return true;
            }
            else if (grabbedItemGO != null)
            {
                Inventory.Instance().RemoveItem(grabbedItemIndex);
                StartCoroutine(DropItem(grabbedItem));
                grabbedItem = null;
                Destroy(grabbedItemGO);
            }
            return true;
        }
        return false;
    }

    private void TakeOrPlaceItem(InventorySlot hitSlot, bool fullPlace)
    {
        if (grabbedItemGO == null && hitSlot != null && hitSlot.currentItem != null)
        {
            GrabItem(hitSlot.currentItem, null);
        }
        else if (grabbedItemGO != null && hitSlot != null && hitSlot.currentItem != null)
        {
            InventoryItem itemInHand = new InventoryItem(grabbedItem.item, grabbedItem.currentStack);
            InventoryItem itemInSlot = new InventoryItem(hitSlot.currentItem.item, hitSlot.currentItem.currentStack);
            if (itemInHand.item != itemInSlot.item)
            {
                PlaceItem(fullPlace);
                GrabItem(itemInSlot, itemInHand);
            }
            else if (itemInSlot.currentStack < itemInSlot.item.maxStack)
            {
                PlaceItem(fullPlace);
                if (grabbedItem != null && grabbedItem.currentStack > 0) { return; }
                grabbedItem = null;
                Destroy(grabbedItemGO);
                grabbedItemGO = null;
            }
        }
        else if (grabbedItemGO != null && hitSlot != null && hitSlot.currentItem == null)
        {
            PlaceItem(fullPlace);
        }
    }

    private void PlaceItem(bool fullPlace)
    {
        if (fullPlace)
        {
            InventoryItem placedItem = Inventory.Instance().UpdateItem(new InventoryItem(grabbedItem.item, grabbedItem.currentStack), grabbedItemIndex);
            SetSpecificSlot(placedItem, grabbedItemIndex);
            grabbedItem = null;
            Destroy(grabbedItemGO);
            grabbedItemGO = null;
        }
        else
        {
            if (grabbedItem.currentStack == 1)
            {
                PlaceItem(true);
                return;
            }
            bool isOdd = grabbedItem.currentStack % 2 != 0;
            grabbedItem.currentStack /= 2;
            InventoryItem placedItem = Inventory.Instance().UpdateItem(new InventoryItem(grabbedItem.item, grabbedItem.currentStack), grabbedItemIndex);
            SetSpecificSlot(placedItem, grabbedItemIndex);
            grabbedItem = new InventoryItem(grabbedItem.item, grabbedItem.currentStack);
            grabbedItem.currentStack += isOdd ? 1 : 0;
        }
    }

    private void GrabItem(InventoryItem invItem, InventoryItem exchangeItem)
    {
        grabbedItem = new InventoryItem(invItem.item, invItem.currentStack);
        grabbedItemGO = new GameObject("Grabbed Item Icon");

        grabbedItemGO.transform.SetParent(transform);
        grabbedItemGO.name = "Grabbed Item Icon";
        Image image = grabbedItemGO.AddComponent<Image>();
        image.sprite = invItem.item.itemSprite;
        image.color = new Color(0.9f, 0.9f, 0.9f);
        image.raycastTarget = false;

        slots[grabbedItemIndex].SetInventorySlot(exchangeItem == null ? null : exchangeItem);
        if (exchangeItem == null)
        {
            Inventory.Instance().RemoveItem(grabbedItemIndex);
        }
        print("Instanitate");
    }

    #endregion UI_Interaction

    #region UIInventory_Functionality

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

    private IEnumerator DropItem(InventoryItem item)
    {
        int stack = item.currentStack;
        for (int i = 0; i < stack; i++)
        {
            Transform target = Inventory.Instance().transform;
            GameObject droppedItem = Instantiate(item.item.itemObject, target.position + Vector3.up * 0.5f + target.forward, Quaternion.identity);
            droppedItem.name = item.item.name;
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion UIInventory_Functionality
}