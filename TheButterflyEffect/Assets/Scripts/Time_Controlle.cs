using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Time_controlle : MonoBehaviour
{
    private TextMeshProUGUI clockText;
    private string minuteText;
    private string hourText;
    [SerializeField] int updateper = 5;
    private int updateer=5;
    [SerializeField] float timeSpeed = 1;
    public float minute = 0;
    public int hour = 0;

    void Awake()
    {
        updateer = updateper;
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
            updateer = updateper;
            hour += 1;
            UpdateClock();
        }
        else
        {
            minute = 0;
            hour = 0;
            updateer = updateper;
            UpdateClock();
        }

        if (Mathf.RoundToInt(minute)>=updateer)
        {
            updateer += updateper;
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
