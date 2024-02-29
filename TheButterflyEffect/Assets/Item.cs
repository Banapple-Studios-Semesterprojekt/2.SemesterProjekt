using UnityEngine;

[CreateAssetMenu(fileName ="New Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public GameObject itemObject;
    public Sprite itemSprite;
    public int maxStack = 1;
}

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int currentStack;

    public InventoryItem(Item item)
    {
        this.item = item;
        currentStack = 1;
    }
}

public enum ItemType
{
    Butterfly,
    Eatable,
    Tools
}