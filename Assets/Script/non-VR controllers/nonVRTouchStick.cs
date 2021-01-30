using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonVRTouchStick : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (other.GetComponent<nonVRGrabableObject>() != null)
            {
                other.GetComponent<nonVRGrabableObject>().Grab();
            }
        }
    }
}
