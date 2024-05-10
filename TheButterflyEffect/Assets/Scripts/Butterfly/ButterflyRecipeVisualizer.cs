using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyRecipeVisualizer : MonoBehaviour
{
    [Header("References")]
    //Has to be dragged in in chronological order
    [SerializeField] private ButterflyImages[] butterflyImages;
    private BreedingSystem breedingSystem; //The scriptable objects

    private List<ButterflyRecipes> revealedRecipes = new List<ButterflyRecipes>();

    private void Start()
    {
        breedingSystem = FindAnyObjectByType<BreedingSystem>();

        Inventory.Instance().onAddItem += Inventory_onAddItem;
    }

    private void Inventory_onAddItem(InventoryItem item, int index)
    {
        if(item.item is ButterflyData)
        {
            DiscoverRecipe((ButterflyData)item.item); //Casting item as butterflyData. 
        }
    }

    public void DiscoverRecipe(ButterflyData butterfly)
    {
        foreach (ButterflyRecipes butterflyRecipe in breedingSystem.GetRecipes())
        {
            if (!revealedRecipes.Contains(butterflyRecipe) && butterflyRecipe.ContainsButterfly(butterfly))
            {
                revealedRecipes.Add(butterflyRecipe);
                UpdateCatalogueRecipe(butterflyRecipe);
                print("Update catalogue recipes");
            }
        }  
    }

    private void UpdateCatalogueRecipe(ButterflyRecipes butterflyRecipe)
    {
        butterflyImages[revealedRecipes.Count - 1].SetButterflyImages(butterflyRecipe.input1.itemSprite, butterflyRecipe.input2.itemSprite, butterflyRecipe.output.itemSprite);
    }
}
