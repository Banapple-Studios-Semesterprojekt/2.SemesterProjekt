using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookScript : MonoBehaviour
{
    private GameObject[] pages;
    private ButterflySlot butterflySlot;

    public Button nextPage;
    public Button prevPage;

    private int pageNumber;
    // Start is called before the first frame update
    void Start()
    {
        //Referencing children in book and converting transform to a gameobject array
        pages = GetComponentsInChildren<Transform>().Where(s => s.name.Contains("Page") && s != transform).Select(t => t.gameObject).ToArray();
        butterflySlot = GetComponentInChildren<ButterflySlot>();

        pageNumber = 0;

        //Button
        nextPage.onClick.AddListener(NextPage);
        prevPage.onClick.AddListener(PreviousPage);

        print("First SetPageActive call");
        SetPageActive();
        UpdatePageNumbers(pageNumber);
    }

    void NextPage()
    {
        if (pageNumber >= pages.Length - 1)
        {  
            pageNumber = 0;
            SetPageActive();
            UpdatePageNumbers(pageNumber);
            print("Go to very first page " + "page number = " + pageNumber);
            return;
        }
        else
        {
            pageNumber++;
            SetPageActive();
            UpdatePageNumbers(pageNumber);
            print("Incease page number, new page number = " + pageNumber);
        }   
    }

    void PreviousPage()
    {
        if (pageNumber <= 0)
        {
            pageNumber = pages.Length - 1;
            SetPageActive();
            UpdatePageNumbers(pageNumber);
            print("Go to the last page, page number = " + pageNumber);
            return;
        }
        else
        {
            pageNumber--;
            SetPageActive();
            UpdatePageNumbers(pageNumber);
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
        //UpdatePageNumbers(pageNumber);
        print("Current page number = " + pageNumber);
    }

    void UpdatePageNumbers(int pageNumber)
    {
        print("Update page numbers, left page number = " + butterflySlot.pageNumberLeft.text + " and right page number = " + butterflySlot.pageNumberRight.text);
        print("(UpdatePageNumbers) Current page number = " + pageNumber);

        int leftPageNumber = pageNumber + 1;
        int rightPageNumber = pageNumber + 2;
        butterflySlot.pageNumberLeft.text = leftPageNumber.ToString();
        butterflySlot.pageNumberRight.text = rightPageNumber.ToString();

        butterflySlot.pageNumberLeft.enabled = true;
        butterflySlot.pageNumberRight.enabled = true;

    }
}
