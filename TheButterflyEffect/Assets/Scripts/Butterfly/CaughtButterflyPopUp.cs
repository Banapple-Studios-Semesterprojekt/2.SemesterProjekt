using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaughtButterflyPopUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image butterflyImage;
    [SerializeField] private TextMeshProUGUI caughtButterflyText;
    [SerializeField] private TextMeshProUGUI andAddedtoCatalogueText;

    private Transform butterflyPopUp;
    private Animator animator;

    private void Start()
    {
        butterflyPopUp = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        butterflyPopUp.gameObject.SetActive(true);
    }

    public void CaughtButterfly(ButterflyData butterfly, ButterflySlot[] butterflySlot)
    {
        for (int i = 0; i < butterflySlot.Length; i++)
        {
            if (!butterflySlot[i].butterflyCaught)
            {
                ActivatePopUp(butterfly);
                andAddedtoCatalogueText.text = "You caught a new butterfly and it has been added to the catalogue";
            }
            else //if (butterflySlot[i].butterflyCaught)
            {
                ActivatePopUp(butterfly);
                caughtButterflyText.text = "You caught a butterfly!";
            }
        }
    }

    public void ActivatePopUp(ButterflyData butterfly)
    {
        butterflyPopUp.gameObject.SetActive(true);
        butterflyImage.sprite = butterfly.itemSprite;
        butterflyImage.enabled = true;
        animator.SetBool("ButterflyCaught", true);
    }
}
