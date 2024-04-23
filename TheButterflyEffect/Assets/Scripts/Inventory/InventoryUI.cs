using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public bool invetoryIsActive = false;
    public InventorySlot[] slots;
    public InventorySlot[] hotbarSlots;
    public InventorySlot[] breedingSlots;
    //the parent object for all inventory slots
    [SerializeField] private Transform slotsParent;
    [SerializeField] private Transform slotsParentHotbar;
    [SerializeField] private Transform slotsParentBreeding;
    [SerializeField] private GameObject breedingUI;
    //canvas references
    private PointerEventData pointerData;
    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;

    //grabbed item properties (the one your holding in the inventory)
    private InventorySlotType slotType;
    private InventoryItem grabbedItem;
    private GameObject grabbedItemGO;
    private int grabbedItemIndex;
    private int inventoryCapacity;

    //events
    public delegate void PlaceItemAction();
    public event PlaceItemAction onPlaceItem;

    private void Awake()
    {
        //Finding all the inventory slots through the parent object
        slots = slotsParent.Find("Inventory Slots").GetComponentsInChildren<InventorySlot>();
        hotbarSlots = slotsParentHotbar.Find("Hotbar Slots").GetComponentsInChildren<InventorySlot>();
        breedingSlots = slotsParentBreeding.GetComponentsInChildren<InventorySlot>();
    }

    private void Start()
    {
        //Subscribing events from Inventory instance
        Inventory.Instance().onAddItem += AddItemEvent;
        Inventory.Instance().onToggleInventory += ToggleInventoryEvent;
        //Finding the event system and graphic raycaster for the canvas
        eventSystem = FindAnyObjectByType<EventSystem>();
        raycaster = transform.GetComponent<GraphicRaycaster>();

        inventoryCapacity = Inventory.Instance().getinventoryCapacity();

        //Resetting all slots (might be changed later for saving data purposes)
        ClearAllSlots();

        //Setting the UI inventory to invisible
        slotsParent.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Inventory.Instance().onAddItem -= AddItemEvent;
        Inventory.Instance().onToggleInventory -= ToggleInventoryEvent;
        PlayerController.playerInput.Player.PrimaryAction.performed -= InventorySlotInteract;
        PlayerController.playerInput.Player.SecondaryAction.performed -= StackSplit;
    }
    private void Update()
    {
        UpdateGrabbedItemToMouse();
    }


    #region Events
    private void AddItemEvent(InventoryItem item, int index)
    {
        slotType = InventorySlotType.InventorySlots;
        SetSpecificSlot(item, index);
        onPlaceItem?.Invoke();
    }
    private void ToggleInventoryEvent(bool isActive)
    {
        if(isActive)
        {
            PlayerController.playerInput.Player.PrimaryAction.performed += InventorySlotInteract;
            PlayerController.playerInput.Player.SecondaryAction.performed += StackSplit;
            invetoryIsActive = true;

        }
        else
        {
            invetoryIsActive = false;
            GetComponent<ChangeUI>().DiactivateCatalogue();
            PlayerController.playerInput.Player.PrimaryAction.performed -= InventorySlotInteract;
            PlayerController.playerInput.Player.SecondaryAction.performed -= StackSplit;

            breedingUI.SetActive(false);

            if(grabbedItemGO)
            {
                Inventory.Instance().RemoveItem(grabbedItemIndex);
                StartCoroutine(DropItem(grabbedItem));
                grabbedItem = null;
                Destroy(grabbedItemGO);
                onPlaceItem?.Invoke();
            }
        }
    }
    #endregion

    #region UI_Interaction
    private void UpdateGrabbedItemToMouse()
    {
        //Making the grabbed item follow the mouse/cursor
        if (grabbedItemGO != null)
        {
            Vector2 mouseDelta = PlayerController.playerInput.Player.CameraLook.ReadValue<Vector2>();
            float xScale = 1 + Mathf.Abs(mouseDelta.x) * 0.1f; xScale = Mathf.Clamp(xScale, 1f, 3f);
            float yScale = 1 + Mathf.Abs(mouseDelta.y) * 0.1f; yScale = Mathf.Clamp(yScale, 1f, 3f);
            float zRot = mouseDelta.x * 4; zRot = Mathf.Clamp(zRot, -20f, 20f);
            grabbedItemGO.transform.position = Mouse.current.position.value;
            grabbedItemGO.transform.localScale = Vector3.Lerp(grabbedItemGO.transform.localScale, new Vector2(xScale, yScale), 10 * Time.deltaTime);
            grabbedItemGO.transform.localRotation = Quaternion.Euler(0f, 0f, zRot);
        }
    }
    private void InventorySlotInteract(InputAction.CallbackContext context)
    {
        //First raycast to any ui graphic, to select an item from the inventory
        List<RaycastResult> results = UIRaycasting();

        //Secondly check if mouse is clicking outside the inventory window. If so, throw out item if is grabbed
        bool isGrabbedandDropping = DropItemIfGrabbed(results.ToArray());
        if(isGrabbedandDropping) { return; }

        //Retrieving the clicked inventory slot
        InventorySlot hitSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
        SetSlotType(hitSlot);

        //If the clicked slot is occupied and no grabbed object, take the object on mouse, else if clicked empty slot with grabbed item, place it
        TakeOrPlaceItem(hitSlot, true);
    }

    private void StackSplit(InputAction.CallbackContext context)
    {
        if(grabbedItemGO != null)
        {
            List<RaycastResult> results = UIRaycasting();
            if(results.Count <= 0) { return; }
            InventorySlot hitSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
            SetSlotType(hitSlot);
            if (hitSlot != null && hitSlot.currentItem != null && hitSlot.currentItem.item != grabbedItem.item) { return; }
            TakeOrPlaceItem(hitSlot, false);
        }
    }

    private void SetSlotType(InventorySlot hitSlot)
    {
        if (slots.Contains(hitSlot))
        {
            grabbedItemIndex = Array.IndexOf(slots, hitSlot);
            slotType = InventorySlotType.InventorySlots;
        }
        else if (hotbarSlots.Contains(hitSlot))
        {
            grabbedItemIndex = Array.IndexOf(hotbarSlots, hitSlot);
            slotType = InventorySlotType.HotbarSlots;
        }
        else if (breedingSlots.Contains(hitSlot))
        {
            grabbedItemIndex = Array.IndexOf(breedingSlots, hitSlot);
            slotType = InventorySlotType.BreedingSlots;
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
                onPlaceItem?.Invoke();
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
            if(itemInHand.item != itemInSlot.item) 
            {
                if(slotType == InventorySlotType.InventorySlots || slotType == InventorySlotType.BreedingSlots || (slotType == InventorySlotType.HotbarSlots && grabbedItem.item.itemType == ItemType.Tools))
                {
                    PlaceItem(fullPlace);
                    GrabItem(itemInSlot, itemInHand);
                }
            }
            else if(itemInSlot.currentStack < itemInSlot.item.maxStack)
            {
                PlaceItem(fullPlace);
                if(grabbedItem != null && grabbedItem.currentStack > 0) { return; }
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
            if (slotType == InventorySlotType.InventorySlots || slotType == InventorySlotType.BreedingSlots || (slotType == InventorySlotType.HotbarSlots && grabbedItem.item.itemType == ItemType.Tools))
            { 
                int i = (slotType == InventorySlotType.InventorySlots || slotType == InventorySlotType.BreedingSlots) ? grabbedItemIndex : grabbedItemIndex + inventoryCapacity - hotbarSlots.Length;
                if(slotType != InventorySlotType.BreedingSlots)
                {
                    InventoryItem placedItem = Inventory.Instance().UpdateItem(new InventoryItem(grabbedItem.item, grabbedItem.currentStack), i);
                }
                SetSpecificSlot(grabbedItem, grabbedItemIndex);
                grabbedItem = null;
                Destroy(grabbedItemGO);
                grabbedItemGO = null;
                onPlaceItem?.Invoke();
            }
        }
        else
        {
            if(grabbedItem.currentStack == 1) 
            {
                PlaceItem(true);
                return;
            }
            else
            {
                bool isOdd = grabbedItem.currentStack % 2 != 0;
                grabbedItem.currentStack /= 2;
                InventoryItem placedItem = Inventory.Instance().UpdateItem(new InventoryItem(grabbedItem.item, grabbedItem.currentStack), grabbedItemIndex);
                SetSpecificSlot(placedItem, grabbedItemIndex);
                grabbedItem = new InventoryItem(grabbedItem.item, grabbedItem.currentStack);
                grabbedItem.currentStack += isOdd ? 1 : 0;
            }
            
        }
        
    }
    private void GrabItem(InventoryItem invItem, InventoryItem exchangeItem)
    {
        if(invItem == null)
        {
            Debug.LogError("No Invetory Item To Grab!");
            return;
        }

        grabbedItem = new InventoryItem(invItem.item, invItem.currentStack);
        grabbedItemGO = new GameObject("Grabbed Item Icon");

        grabbedItemGO.transform.SetParent(transform);
        grabbedItemGO.name = "Grabbed Item Icon";
        Image image = grabbedItemGO.AddComponent<Image>();
        image.sprite = invItem.item.itemSprite;
        image.color = new Color(0.9f, 0.9f, 0.9f);
        image.raycastTarget = false;

        if (slotType == InventorySlotType.InventorySlots)
        {
            slots[grabbedItemIndex].SetInventorySlot(exchangeItem);
        }
        else if (slotType == InventorySlotType.HotbarSlots)
        {
            hotbarSlots[grabbedItemIndex].SetInventorySlot(exchangeItem);
        }
        else if(slotType == InventorySlotType.BreedingSlots)
        {
            breedingSlots[grabbedItemIndex].SetInventorySlot(exchangeItem);
        }
        
        if(exchangeItem == null && slotType != InventorySlotType.BreedingSlots)
        {
            int i = slotType == InventorySlotType.InventorySlots ? grabbedItemIndex : grabbedItemIndex + inventoryCapacity - hotbarSlots.Length;
            Inventory.Instance().RemoveItem(i);
        }
        print("Instanitate");
    }

    #endregion

    #region UIInventory_Functionality
    public void ClearAllSlots()
    {
        foreach (InventorySlot item in slots)
        {
            item.SetInventorySlot(null);
        }
        foreach (InventorySlot item in hotbarSlots)
        {
            item.SetInventorySlot(null);
        }

        foreach (InventorySlot item in breedingSlots)
        {
            item.SetInventorySlot(null);
        }
    }

    public void SetSpecificSlot(InventoryItem item, int index)
    {
        if (slotType == InventorySlotType.InventorySlots)
        {
            if(index > 11) { hotbarSlots[index - inventoryCapacity + 3].SetInventorySlot(item); return; }
            slots[index].SetInventorySlot(item);
        }
        else if(slotType == InventorySlotType.HotbarSlots)
        {
            Debug.Log(index);
            hotbarSlots[index].SetInventorySlot(item);
        }
        else if(slotType == InventorySlotType.BreedingSlots)
        {
            breedingSlots[index].SetInventorySlot(item);
        }
    }

    IEnumerator DropItem(InventoryItem item)
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

    public InventorySlot[] GetHotbarSlots() { return hotbarSlots; }
    #endregion
}

public enum InventorySlotType
{
    InventorySlots,
    HotbarSlots,
    BreedingSlots
}