using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreedingSystem : MonoBehaviour
{
    [SerializeField] private InventorySlot slot1, slot2, result;
    [SerializeField] private ButteflyRecipe[] recipies;
    [SerializeField] private Slider BreedSlider;

    private void Start()
    {
        slot1.onSlotChange += OnSlotChange;
        slot2.onSlotChange += OnSlotChange;
    }

    private void OnSlotChange()
    {
        print("Invetory Slot Changed!");
    }

    public void TryBreed()
    {
        foreach (ButteflyRecipe recipe in recipies)
        {
            ButterflyData output = TryGetOutput(recipe);
            if (output != null)
            {
                StartCoroutine(Breed(recipe, output));
            }
        }
    }

    IEnumerator Breed(ButteflyRecipe butteflyRecipe, ButterflyData output)
    {
        float bredingtime = 0;
        while (bredingtime < butteflyRecipe.breedTime)
        {
            yield return new WaitForSeconds(0.05f);
            bredingtime += 0.05f;
            BreedSlider.value = bredingtime / butteflyRecipe.breedTime;
        }
        InventoryItem outputItem = new InventoryItem(output);
        result.SetInventorySlot(outputItem);
        ClearInputSlots();
        BreedSlider.value = 0;
        print("Sets Inventory Output Slot!");
    }

    public void ClearInputSlots()
    {
        slot1.RemoveInventorySlot();
        slot2.RemoveInventorySlot();
    }

    ButterflyData TryGetOutput(ButteflyRecipe recipe)
    {
        if (slot1.currentItem != null && slot2.currentItem != null)
        {
            if ((recipe.input1 == slot1.currentItem.item || recipe.input1 == slot2.currentItem.item) && (recipe.input2 == slot1.currentItem.item || recipe.input2 == slot2.currentItem.item))
            {
                print("Recipe gives output!");
                return recipe.output;
            }
        }

        print("Recipe gives null...");
        return null;
    }
}
