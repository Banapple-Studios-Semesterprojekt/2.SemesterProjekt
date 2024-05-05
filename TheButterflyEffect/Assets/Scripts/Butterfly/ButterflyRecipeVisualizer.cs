using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyRecipeVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ButterflyRecipes[] butterflyRecipes; //The scriptable objects
    private ButterflyIngredients butterflyIngredients; //The butterfly recipe slots
    private List<GameObject> butterflyResults = new List<GameObject>();

    private void ButterflyRecipe(ButterflyData butterfly)
    {
        
    }

    
}
