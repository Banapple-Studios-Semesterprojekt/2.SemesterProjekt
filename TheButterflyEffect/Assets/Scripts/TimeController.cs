using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private TextMeshProUGUI clockText;
    private string minuteText;
    private string hourText;
    [SerializeField] int updateDelay = 5;
    private int updater=5;
    [SerializeField] float timeSpeed = 1;
    public float minute = 0;
    public int hour = 0;

    void Awake()
    {
        updater = updateDelay;
        clockText=gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (minute<59)
        {
            minute += timeSpeed*Time.deltaTime;
        }
        else if(hour<23)
        {
            minute = 0;
            updater = updateDelay;
            hour += 1;
            UpdateClock();
        }
        else
        {
            minute = 0;
            hour = 0;
            updater = updateDelay;
            UpdateClock();
        }

        if (Mathf.RoundToInt(minute)>=updater)
        {
            updater += updateDelay;
            UpdateClock();
        }
    }
    void UpdateClock()
    {
        if (minute < 9)
        {
            minuteText = ":0" + Mathf.RoundToInt(minute);
        }
        else { minuteText = ":" + Mathf.RoundToInt(minute); }

        if (hour < 10)
        {
            hourText = "0" + hour;
        }
        else { hourText = "" + hour; }

        clockText.text = hourText + minuteText;
    }

}