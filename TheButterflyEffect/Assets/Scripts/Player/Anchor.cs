using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : ItemMechanic
{
    public delegate void PlaceAnchor();
    public event PlaceAnchor onPlace;
    [SerializeField] Hotbar hotbar;
    private bool canplace=false;
    private void OnEnable()
    {
        canplace=false;
        Invoke("setcanplace", .2f);
        heldItemScript.onPrimaryAction += Place;
    }
    private void setcanplace()
    {
        canplace = true;

    }
    private void OnDisable()
    {
        heldItemScript.onPrimaryAction -= Place;
    }

    private void Place()
    {
            if(canplace)
            { 
                Inventory.Instance().PlaceItem(12+hotbar.selectedSlot);
            }
            

    }
}
