using System;
using System.Collections.Generic;
using UnityEngine;


public class HeldItem : MonoBehaviour
{
    [SerializeField] private GameObject[] obj_items;
    private Item[] items;
    private Interactor interactor;


    private void Start()
    {
        interactor=GetComponent<Interactor>();
        List<Item> itemMechs = new List<Item>();
        foreach (GameObject obj in obj_items)
        {
            itemMechs.Add(obj.GetComponent<ItemMechanic>().item);
        }
        items = itemMechs.ToArray();

        FindAnyObjectByType<Hotbar>().onSlotSelect += Hotbar_OnSlotSelect;
        Hotbar_OnSlotSelect(null);
    }

    private void Hotbar_OnSlotSelect(InventoryItem item)
    {
        for (int i = 0; i < obj_items.Length; i++)
        {
            if (item != null && items[i] == item.item) { continue; }
            obj_items[i].SetActive(false);
        }
        interactor.enabled = item == null;
        if (item == null) { return; }
        int itemIndex = Array.IndexOf(items, item.item);
        obj_items[itemIndex].SetActive(true);
    }
}
