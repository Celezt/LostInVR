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
            Debug.Log("Trying to grab.");
            if (other.GetComponent<nonVRGrabableObject>() != null)
            {
                Debug.Log("Grabbing!!!");
                other.GetComponent<nonVRGrabableObject>().Grab();
            }
        }
    }
}
