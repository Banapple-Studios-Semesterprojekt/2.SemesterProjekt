using UnityEngine;

public class ButterflyCatalogue : MonoBehaviour
{
    private ButterflySlot[] butterflySlot;
    private CaughtButterflyPopUp butterflyPopUp;

    private void Start()
    {
        Inventory.Instance().onAddItem += ButterflyCatalogue_onAddItem;
        butterflySlot = transform.GetChildrenRecursive<ButterflySlot>().ToArray();
        butterflyPopUp = FindAnyObjectByType<CaughtButterflyPopUp>();
    }

    private void ButterflyCatalogue_onAddItem(InventoryItem item, int index)
    { 
        if(item.item is ButterflyData)
        {
            ButterflyData butterfly = item.item as ButterflyData;
            //Debug.Log("Butterfly name = " + butterfly.butterflyName + "\n" + "Butterfly description = " + butterfly.description + "\n" + "Butterfly spawn probability = " +
            //butterfly.spawnProbability + "\n" + "Butterfly sprite = " + butterfly.itemSprite);

            for (int i = 0; i < butterflySlot.Length; i++)
            {   //Ensures there are no duplications of butterflies in the catalogue.
                if(butterflySlot[i].butterflyData == butterfly)
                {
                    butterflyPopUp.CaughtButterfly(butterfly, true);
                    return;
                }
                else if (!butterflySlot[i].butterflyCaught)
                {
                    butterflySlot[i].AddButterflyToSlot(butterfly);
                    butterflyPopUp.CaughtButterfly(butterfly, false);
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

}
