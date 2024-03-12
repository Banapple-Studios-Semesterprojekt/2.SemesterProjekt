using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Time_controlle : MonoBehaviour
{
    private TextMeshProUGUI clockText;
    private string minuteText;
    private string hourText;
    [SerializeField] float timeSpeed = 1;
    public float temptime=0;
    public int minute = 0;
    public int hour = 0;
    void Awake()
    {
        clockText=gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (minute<59)
        {
            temptime+= timeSpeed*Time.deltaTime;
            minute = Mathf.RoundToInt(temptime);

        }
        else if(hour<23)
        {

            temptime = 0;
            minute = 0;
            hour += 1;
        }
        else
        {
            temptime = 0;
            minute = 0;
            hour = 0;
        }
        if (minute<10){ minuteText = ":0"+minute;
        }else { minuteText = ":"+minute; }
        
        if (hour < 10){hourText = "0" + hour;
        }else { hourText = ""+hour; }
        
        clockText.text = hourText + minuteText;
    }
}
