using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookScript : MonoBehaviour
{
    private GameObject[] pages;

    public Button nextPage;
    public Button prevPage;

    private int pageNumber;
    // Start is called before the first frame update
    void Start()
    {
        //Referencing children in book and converting transform to a gameobject array
        pages = GetComponentsInChildren<Transform>().Where(s => s.name.Contains("Page") && s != transform).Select(t => t.gameObject).ToArray();
        pageNumber = 0;

        //Button
        nextPage.onClick.AddListener(NextPage);
        prevPage.onClick.AddListener(PreviousPage);

        print("First SetPageActive call");
        SetPageActive();
    }

    void NextPage()
    {
        if (pageNumber >= pages.Length - 1)
        {  
            pageNumber = 0;
            SetPageActive();
            print("Go to very first page " + "page number = " + pageNumber);
            return;
        }
        else
        {
            pageNumber++;
            SetPageActive();
            print("Incease page number, new page number = " + pageNumber);
        }   
    }

    void PreviousPage()
    {
        if (pageNumber <= 0)
        {
            pageNumber = pages.Length - 1;
            SetPageActive();
            print("Go to the last page, page number = " + pageNumber);
            return;
        }
        else
        {
            pageNumber--;
            SetPageActive();
            print("Decrease page number, page number = " + pageNumber);
        }
    }

    void SetPageActive()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            print("Deactivate pages " + pages[i].name);
        }
        pages[pageNumber].SetActive(true);
        print("Current page number = " + pageNumber);
    }
}
