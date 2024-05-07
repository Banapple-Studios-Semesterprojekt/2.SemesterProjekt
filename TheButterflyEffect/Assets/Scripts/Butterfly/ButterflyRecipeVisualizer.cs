using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyRecipeVisualizer : MonoBehaviour
{
    [Header("References")]
    private ButterflyImages butterflyImages;
    public ButterflyRecipes[] butterflyRecipes; //The scriptable objects
    private GameObject[] recipes;

    //private List<GameObject> butterflyResults = new List<GameObject>();

    private void Start()
    {
        recipes = GetComponentsInChildren<Transform>().Where(s => s.name.Contains("Recipes") && s != transform).Select(t => t.gameObject).ToArray();
    }

    public void ButterflyRecipe(ButterflyRecipes[] butterflyRecipes, ButterflyData butterfly)
    {
        CheckInput1(butterflyRecipes, butterfly);
        CheckInput2(butterflyRecipes, butterfly);
        RevealRecipe(butterflyRecipes, butterfly);


        /*for (int i = 0; i < butterflyRecipes.Length; i++)
        {
            for (int j = 0; j < butterfly.Length; j++)
            {
                if (butterfly[i] == butterflyRecipes[i].input1)
                {
                    butterfly[i].itemSprite = firstButterfly.sprite;
                }

                if (butterfly[i] == butterflyRecipes[i].input2)
                {
                    butterfly[i].itemSprite = secondButterfly.sprite;
                }
        
            }
           
            if (butterfly[i] != null)
            {

            }

        }*/
    }

    public bool CheckInput1(ButterflyRecipes[] butterflyRecipes, ButterflyData butterfly1)
    {
        print("Input1");
        for (int i = 0; i < butterflyRecipes.Length; i++)
        {
            if (butterfly1 == butterflyRecipes[i].input1)
            {
                butterfly1.itemSprite = butterflyImages.firstButterfly.sprite;
                print("First butterfly");
                return true;
            }
        }
        return false;
        
    }

    public bool CheckInput2(ButterflyRecipes[] butterflyRecipes, ButterflyData butterfly2)
    {
        print("Input2");
        for (int i = 0; i < butterflyRecipes.Length; i++)
        {
            if (butterfly2 == butterflyRecipes[i].input2)
            {
                butterfly2.itemSprite = butterflyImages.secondButterfly.sprite;
                print("Second butterfly");
                return true;
            }
        }
        return false;

    }

    public void RevealRecipe(ButterflyRecipes[] butterflyRecipes, ButterflyData butterflyResult)
    {
        print("Result");
        if ((CheckInput1(butterflyRecipes, butterflyResult) && CheckInput2(butterflyRecipes, butterflyResult)) == true)
        {
            butterflyResult.itemSprite = butterflyImages.butterflyResult.sprite;
            print("Result butterfly");
        }

    }




}
