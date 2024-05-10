using UnityEngine;
using UnityEngine.UI;

public class ButterflyImages : MonoBehaviour
{
    [Header("Butterfly images")]
    public Image firstButterfly;
    public Image secondButterfly;
    public Image butterflyResult;

    public void SetButterflyImages(Sprite firstButterfly, Sprite secondButterfly, Sprite butterflyResult)
    {
        this.firstButterfly.sprite = firstButterfly;
        this.secondButterfly.sprite = secondButterfly;
        this.butterflyResult.sprite = butterflyResult;
    }

}
