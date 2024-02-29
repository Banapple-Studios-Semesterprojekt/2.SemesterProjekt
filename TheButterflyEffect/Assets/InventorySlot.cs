using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemLogo;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    public InventoryItem currentItem { get; private set; }

    public void SetInventorySlot(InventoryItem invItem)
    {
        if(invItem == null)
        {
            currentItem = null;
            itemLogo.enabled = false;
            itemQuantityText.text = string.Empty;
            return;
        }
        currentItem = invItem;
        itemLogo.enabled = true;
        itemLogo.sprite = invItem.item.itemSprite;
        itemQuantityText.text = invItem.currentStack <= 1 ? string.Empty : invItem.currentStack.ToString();
    }

    public void RemoveInventorySlot()
    {
        if(currentItem != null)
        {
            currentItem = null;
            itemLogo.sprite = null;
            itemQuantityText.text = string.Empty;
        }
    }
}
