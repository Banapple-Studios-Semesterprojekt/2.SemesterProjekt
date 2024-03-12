using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Day_Night_LightControlle : MonoBehaviour
{
    private Time_controlle tm;

    void Start()
    {
        tm = Inventory.Instance().GetComponentInChildren<Time_controlle>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles=new Vector3(((tm.hour * 60 + tm.temptime) / 4)-90,0,0);
    }
}
