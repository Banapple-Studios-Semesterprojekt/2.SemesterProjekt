using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreedingSystem : MonoBehaviour
{
    [SerializeField] private InventorySlot slot1, slot2, result;
    [SerializeField] private ButterflyRecipes[] recipies;
    [SerializeField] private Slider BreedSlider;

    private Coroutine breedCoroutine;

    private int breedCount = 0;
    public delegate void BreedAction(int breedCount);
    public event BreedAction onBreed;

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
        foreach (ButterflyRecipes recipe in recipies)
        {
            ButterflyData output = TryGetOutput(recipe);
            if (output != null && breedCoroutine == null)
            {
                breedCoroutine = StartCoroutine(Breed(recipe, output));
            }
        }
    }

    IEnumerator Breed(ButterflyRecipes butteflyRecipe, ButterflyData output)
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
        onBreed?.Invoke(++breedCount);
        print("Sets Inventory Output Slot!");
        breedCoroutine = null;
    }

    public void ClearInputSlots()
    {
        slot1.RemoveInventorySlot();
        slot2.RemoveInventorySlot();
    }

    ButterflyData TryGetOutput(ButterflyRecipes recipe)
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
