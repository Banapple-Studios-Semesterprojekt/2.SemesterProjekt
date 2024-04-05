using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HeldItem : MonoBehaviour
{
    [SerializeField] private GameObject[] obj_items;
    private Item[] items;
    private Interactor interactor;

    //Events
    public delegate void primaryAction();
    public event primaryAction onPrimaryAction;
    public delegate void secondaryAction();
    public event secondaryAction onSecondaryAction;

    public delegate void HoldItemAction(string type);
    public event HoldItemAction onHoldItem;

    private void Start()
    {
        interactor = GetComponent<Interactor>();
        List<Item> itemMechs = new List<Item>();
        foreach (GameObject obj in obj_items)
        {
            itemMechs.Add(obj.GetComponent<ItemMechanic>().item);
        }
        items = itemMechs.ToArray();

        FindAnyObjectByType<Hotbar>().onSlotSelect += Hotbar_OnSlotSelect;
        Hotbar_OnSlotSelect(null);

        //Subscribe player input; primary and secondary actions (Don't have to do it again for held items).
        PlayerController.playerInput.Player.PrimaryAction.performed += PrimaryAction;
        PlayerController.playerInput.Player.SecondaryAction.performed += SecondaryAction;
    }


    private void PrimaryAction(InputAction.CallbackContext obj)
    {
        onPrimaryAction?.Invoke();
    }

    private void SecondaryAction(InputAction.CallbackContext obj)
    {
        onSecondaryAction?.Invoke(); //Toggle or hold for glowstick??
    }

    private void Hotbar_OnSlotSelect(InventoryItem item)
    {
        for (int i = 0; i < obj_items.Length; i++)
        {
            if (item != null && items[i] == item.item) { continue; }
            obj_items[i].SetActive(false);
        }
        interactor.canRaycast = item == null;
        if (item == null) 
        {
            onHoldItem?.Invoke(null);
            return; 
        }
        int itemIndex = Array.IndexOf(items, item.item);
        obj_items[itemIndex].SetActive(true);
        onHoldItem?.Invoke(items[itemIndex].name);
    }
}
