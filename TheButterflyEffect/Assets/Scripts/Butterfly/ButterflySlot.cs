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
    public bool butterflyCaught;

    public void AddButterflyToSlot(ButterflyData newButterfly)
    {
        //Setting variables of this script = the newly caught butterfly data.
        //By using the data from the scriptable objects
        butterflyName.text = newButterfly.butterflyName;
        butterflyDescription.text = newButterfly.description;
        butterflyImage.sprite = newButterfly.itemSprite;
        Debug.Log(newButterfly.itemSprite.name);

        //Enabling "butterflyImage"
        butterflyImage.enabled = true;

        //Setting bool "isCaught" to true, so only one can be caught
        butterflyCaught = true;
    }

    

}
