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

    private Transform butterflyPopUp;
    private Animator animator;

    private void Start()
    {
        butterflyPopUp = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        butterflyPopUp.gameObject.SetActive(true);
    }

    public void CaughtButterfly(ButterflyData butterfly, bool alreadyExists)
    {
        ActivatePopUp(butterfly);
        caughtButterflyText.text = alreadyExists ? "You caught a butterfly!" : "You caught a new butterfly and it has been added to the catalogue";
    }

    public void ActivatePopUp(ButterflyData butterfly)
    {
        butterflyPopUp.gameObject.SetActive(true);
        butterflyImage.sprite = butterfly.itemSprite;
        butterflyImage.enabled = true;
        animator.SetTrigger("ButterflyCaught");
    }
}
