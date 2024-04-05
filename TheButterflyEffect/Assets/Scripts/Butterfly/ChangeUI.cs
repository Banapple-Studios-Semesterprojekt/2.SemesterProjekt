using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject catalogueUI;
    [SerializeField] private GameObject hotbar;

    public Button butterflyCatalogue;
    public Button inventory;

    private void Start()
    {
        //Button clicks
        butterflyCatalogue.onClick.AddListener(ChangeToCatalogue);
        inventory.onClick.AddListener(ChangeToInventory);
    }

    void ChangeToInventory()
    {
        inventoryUI.SetActive(true);
        hotbar.SetActive(true);
        catalogueUI.SetActive(false);
    }

    void ChangeToCatalogue()
    {
        inventoryUI.SetActive(false);
        hotbar.SetActive(false);
        catalogueUI.SetActive(true);    
    }

}
