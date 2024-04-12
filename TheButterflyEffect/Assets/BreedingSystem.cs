using UnityEngine;

public class BreedingSystem : MonoBehaviour
{
    [SerializeField] private InventorySlot slot1, slot2, result;
    [SerializeField] private ButteflyRecipe[] recipies;

    private void Start()
    {
        slot1.onSlotChange += OnSlotChange;
        slot2.onSlotChange += OnSlotChange;
    }

    private void OnSlotChange()
    {
        print("Invetory Slot Changed!");
        Invoke(nameof(TryBreed), 0.2f);
    }

    public void TryBreed()
    {
        foreach (ButteflyRecipe recipe in recipies)
        {
            ButterflyData output = TryGetOutput(recipe);
            if(output != null)
            {
                InventoryItem outputItem = new InventoryItem(output);
                result.SetInventorySlot(outputItem);
                ClearInputSlots();
                print("Sets Inventory Output Slot!");
            }
        }
    }

    public void ClearInputSlots()
    {
        slot1.RemoveInventorySlot();
        slot2.RemoveInventorySlot();
    }

    ButterflyData TryGetOutput(ButteflyRecipe recipe)
    {
        if(slot1.currentItem != null && slot2.currentItem != null)
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
