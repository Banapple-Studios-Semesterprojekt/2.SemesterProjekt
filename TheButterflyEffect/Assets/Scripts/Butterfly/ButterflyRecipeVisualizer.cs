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
    private BookScript bookScript;

    private int recipesIndex;
    //private List<GameObject> butterflyResults = new List<GameObject>();

    private void Start()
    {
        recipes = GetComponentsInChildren<Transform>().Where(s => s.name.Contains("Recipes") && s != transform).Select(t => t.gameObject).ToArray();
        bookScript = FindAnyObjectByType<BookScript>();

        bookScript.nextPage.onClick.AddListener(NextRecipes);
        bookScript.prevPage.onClick.AddListener(PreviousRecipes);
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
        for (int i = 0;i < butterflyRecipes.Length; i++)
        {
            if ((CheckInput1(butterflyRecipes, butterflyResult) && CheckInput2(butterflyRecipes, butterflyResult)) == true)
            {
                butterflyResult.itemSprite = butterflyImages.butterflyResult.sprite;
                butterflyRecipes[i].recipeUnlocked = true;
                print("Result butterfly");
            }
        }
        

    }

    //===================SETTING THE RECIPE PAGES TO TRUE=====================//
    void NextRecipes()
    {
        if (recipesIndex >= recipes.Length - 1)
        {
            recipesIndex = 0;
            RecipePages();
            //print("Go to very first recipes " + "recipes number = " + recipesNumber);
            return;
        }
        else
        {
            recipesIndex++;
            RecipePages();
            //print("Incease recipes number, new recipes number = " + recipesNumber);
        }
    }

    void PreviousRecipes()
    {
        if (recipesIndex <= 0)
        {
            recipesIndex = recipes.Length - 1;
            RecipePages();
            //print("Go to the last recipes, recipes number = " + recipesNumber);
            return;
        }
        else
        {
            recipesIndex--;
            RecipePages();
            //print("Decrease recipes number, recipes number = " + recipesNumber);
        }
    }

    public void RecipePages()
    {
        //Sets all normal pages to inactive
        for (int p = 0; p < bookScript.GetPages().Length; p++)
        {
            if (bookScript.GetPages()[p] == null)
            {
                bookScript.GetPages()[p].SetActive(false);
            }
        }

        //Sets recipe pages to active
        for (int i = 0; i < recipes.Length; i++)
        {
            recipes[i].SetActive(false);
        }
        recipes[bookScript.GetPageIndex()].SetActive(true);
    }

}
