using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemBackground;
    [SerializeField] private Image itemLogo;
    [SerializeField] private TextMeshProUGUI itemQuantityText;

    private Color normalColor;
    private Color highlightColor;

    public InventoryItem currentItem { get; private set; }

    private void Start()
    {
        normalColor = itemBackground.color;
        highlightColor = itemBackground.color * 1.4f;
    }

    public InventoryItem SetInventorySlot(InventoryItem invItem)
    {
        if(invItem == null)
        {
            currentItem = null;
            itemLogo.enabled = false;
            itemQuantityText.text = string.Empty;
            return null;
        }
        currentItem = invItem;
        itemLogo.enabled = true;
        itemLogo.sprite = invItem.item.itemSprite;
        itemQuantityText.text = invItem.currentStack <= 1 ? string.Empty : invItem.currentStack.ToString();
        return currentItem;
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

    public void HighlightInventorySlot(bool highlight)
    {
        itemBackground.color = highlight ? highlightColor : normalColor;
    }
}
