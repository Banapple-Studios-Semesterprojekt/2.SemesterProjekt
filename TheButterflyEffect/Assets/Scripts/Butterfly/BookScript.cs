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
    public TextMeshProUGUI pageNumberRight;
    public TextMeshProUGUI pageNumberLeft;

    private int pageIndex;
    private int pageCount;
    // Start is called before the first frame update
    void Start()
    {
        //Referencing children in book and converting transform to a gameobject array
        pages = GetComponentsInChildren<Transform>().Where(s => s.name.Contains("Page") && s != transform).Select(t => t.gameObject).ToArray();
        butterflySlot = GetComponentInChildren<ButterflySlot>();

        pageIndex = 0;
        pageCount = 2;

        //Button
        nextPage.onClick.AddListener(NextPage);
        prevPage.onClick.AddListener(PreviousPage);

        //print("First SetPageActive call");
        SetPageActive();
        UpdatePageNumbers();
    }

    void NextPage()
    {
        if (pageIndex >= pages.Length - 1)
        {  
            pageIndex = 0;
            pageCount = 2;
            SetPageActive();   
            //print("Go to very first page " + "page number = " + pageNumber);
            return;
        }
        else
        {
            pageIndex++;
            pageCount += 2;
            SetPageActive();  
            //print("Incease page number, new page number = " + pageNumber);
        }   
    }

    void PreviousPage()
    {
        if (pageIndex <= 0)
        {
            pageIndex = pages.Length - 1;
            pageCount = pages.Length * 2;
            SetPageActive(); 
            //print("Go to the last page, page number = " + pageNumber);
            return;
        }
        else
        {
            pageIndex--;
            pageCount -= 2;
            SetPageActive();
            //print("Decrease page number, page number = " + pageNumber);
        }
    }

    void SetPageActive()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            //print("Deactivate pages " + pages[i].name);
        }
        pages[pageIndex].SetActive(true);
        UpdatePageNumbers();
    }

    void UpdatePageNumbers()
    {
        int leftPageNumber = pageCount - 1;
        int rightPageNumber = pageCount;
        pageNumberLeft.text = leftPageNumber.ToString();
        pageNumberRight.text = rightPageNumber.ToString();

        pageNumberLeft.enabled = true;
        pageNumberRight.enabled = true;

    }

}
