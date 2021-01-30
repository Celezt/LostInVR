using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSensor : MonoBehaviour
{
    public int ItemsOnFloor = 0;
    public int RecentItemCount = 0;
    public float Timer;
    public float TimerTime = 5;

    void Update()
    {
        if (Timer > 0f)
        {
            Timer -= Time.deltaTime; if (Timer <= 0f)
            {
                RecentItemCount = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Object")
        {
            ItemsOnFloor += 1;
            RecentItemCount += 1;
            if (Timer <= 0f)
            {
                Timer = TimerTime;
            }
        }
    }
}