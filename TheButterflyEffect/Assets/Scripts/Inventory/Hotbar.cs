using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hotbar : MonoBehaviour
{
    private InventorySlot[] hotbarSlots;
    private int selectedSlot = 0;
    public event Action<InventoryItem> onSlotSelect;

    private void Start()
    {
        hotbarSlots = GetComponent<InventoryUI>().GetHotbarSlots();
        GetComponent<InventoryUI>().onPlaceItem += InventoryUI_OnPlaceItem;
        SelectSlot();
    }

    private void InventoryUI_OnPlaceItem()
    {
        onSlotSelect?.Invoke(hotbarSlots[selectedSlot].currentItem);
    }

    private void Update()
    {
        SelectInput();
    }

    private void SelectSlot()
    {
        foreach (InventorySlot slot in hotbarSlots)
        {
            slot.HighlightInventorySlot(false);
        }

        hotbarSlots[selectedSlot].HighlightInventorySlot(true);
        onSlotSelect?.Invoke(hotbarSlots[selectedSlot].currentItem);
    }

    private void SelectInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) { selectedSlot = 0; SelectSlot(); }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) { selectedSlot = 1; SelectSlot(); }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) { selectedSlot = 2; SelectSlot(); }

        if(Mouse.current.scroll.value.y < 0) 
        {
            if(selectedSlot >= hotbarSlots.Length - 1)
            {
                selectedSlot = 0;
                SelectSlot();
                return;
            }
            selectedSlot++;
            SelectSlot();
        }
        else if(Mouse.current.scroll.value.y > 0)
        {
            if(selectedSlot <= 0)
            {
                selectedSlot = hotbarSlots.Length - 1;
                SelectSlot();
                return;
            }
            selectedSlot--;
            SelectSlot();
        }
    }

    public InventoryItem GetSelectedItem()
    {
        return hotbarSlots[selectedSlot].currentItem;
    }
}
