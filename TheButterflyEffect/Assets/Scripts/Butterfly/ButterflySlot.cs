using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButterflySlot : MonoBehaviour
{
    //==========ITEM DATA==========//
    ButterflyData butterfly; //All information is in the scriptable object.


    //==========BUTTERFLY SLOT===========//
    public TextMeshProUGUI butterflyDescription;
    public TextMeshProUGUI butterflyName;
    public Image butterflyImage;
    public Image hideButterflySprite;
    public bool butterflyCaught;

    //In the beginning there are no butterflies
    public void ZeroButterflies()
    {
        butterflyImage.sprite = null;
        butterflyImage.enabled = false;
        hideButterflySprite.enabled = true;
    }

    public void AddButterflyToSlot(ButterflyData newButterfly)
    {
        Debug.Log("Bye");
        //Setting variables of this script = the newly caught butterfly data.
        //By using the data from the scriptable objects
        butterflyName.text = newButterfly.butterflyName;
        butterflyDescription.text = newButterfly.description;
        butterflyImage.sprite = newButterfly.itemSprite;
        Debug.Log(newButterfly.itemSprite.name);
        //Disabling "hideButterflySprite" 
        hideButterflySprite.enabled = false;

        //Enabling "butterflyImage"
        butterflyImage.enabled = true;

        //Setting bool "isCaught" to true, so only one can be caught
        butterflyCaught = true;
    }
}
