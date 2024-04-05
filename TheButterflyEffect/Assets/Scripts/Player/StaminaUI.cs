using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private GameObject staminaBarParent;

    private void Start()
    {
        Inventory.Instance().GetComponent<StaminaSadFace>().onStaminaUpdate += SetStaminaBar;
    }

    public void SetStaminaBar(float stamina, bool visible)
    {
        if(!enabled) return;
        staminaBar.fillAmount = stamina;
        staminaBarParent.SetActive(visible);
    }
}
