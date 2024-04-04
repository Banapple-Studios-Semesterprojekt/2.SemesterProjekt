using UnityEngine;

public class DayNightLightController : MonoBehaviour
{
    private TimeController tm;

    void Start()
    {
        tm = Inventory.Instance().GetComponentInChildren<TimeController>();
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(((tm.hour * 60 + tm.minute) / 4)-90,0,0);
    }
}
