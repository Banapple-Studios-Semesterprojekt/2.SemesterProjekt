using UnityEngine;

public class ButterflyCatalogue : MonoBehaviour
{
    private ButterflySlot[] butterflySlot;
    private ButterflyRecipeVisualizer butterflyRecipeVisualizer;
    private ButterflyRecipes butterflyRecipes;
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
            {
                if (!butterflySlot[i].butterflyCaught && !butterflyRecipes.recipeUnlocked)
                {
                    butterflySlot[i].AddButterflyToSlot(butterfly);
                    butterflyPopUp.CaughtButterfly(butterfly, butterflySlot);
                    butterflyRecipeVisualizer.ButterflyRecipe(butterflyRecipeVisualizer.butterflyRecipes, butterfly);
                    print("Show ingredients");
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
