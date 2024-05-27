using UnityEngine;

public class ButterflyCatalogue : MonoBehaviour
{
    private ButterflySlot[] butterflySlot;
    private CaughtButterflyPopUp butterflyPopUp;

    private void Start()
    {
        Inventory.Instance().onAddItem += ButterflyCatalogue_onAddItem;
        FindAnyObjectByType<BreedingSystem>().onBreed += ButterflyCatalogue_onBreed;
        butterflySlot = transform.GetChildrenRecursive<ButterflySlot>().ToArray();
        butterflyPopUp = FindAnyObjectByType<CaughtButterflyPopUp>();
    }

    private void ButterflyCatalogue_onBreed(int breedCount, Item item)
    {
        if (item is ButterflyData)
        {
            AddToCatalogue(item as ButterflyData);
        }
        else
        {
            return;
        }
    }

    private void ButterflyCatalogue_onAddItem(InventoryItem item, int index)
    { 
        if(item.item is ButterflyData)
        {
            AddToCatalogue(item.item as ButterflyData);
        }
        else
        {
            return;
        }
    }

    private void AddToCatalogue(ButterflyData data)
    {
        //Debug.Log("Butterfly name = " + butterfly.butterflyName + "\n" + "Butterfly description = " + butterfly.description + "\n" + "Butterfly spawn probability = " +
        //butterfly.spawnProbability + "\n" + "Butterfly sprite = " + butterfly.itemSprite);

        for (int i = 0; i < butterflySlot.Length; i++)
        {   //Ensures there are no duplications of butterflies in the catalogue.
            if (butterflySlot[i].butterflyData == data)
            {
                butterflyPopUp.CaughtButterfly(data, true);
                return;
            }
            else if (!butterflySlot[i].butterflyCaught)
            {
                butterflySlot[i].AddButterflyToSlot(data);
                butterflyPopUp.CaughtButterfly(data, false);
                return;
            }
        }
    }
}
