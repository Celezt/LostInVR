using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonVRGrabableObject : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject myBody;
    bool hasPlayer = false;
    bool beingCarried = false;
    bool touched = false;

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("PlayerNonVRCamera").transform;
    }

    void Update()
    {
        if (beingCarried)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Grab Released.");
                myBody.GetComponent<Rigidbody>().isKinematic = false;
                myBody.transform.parent = null;
                beingCarried = false;
                touched = false;
            }
        }
    }

    public void Grab()
    {
        Debug.Log("Grab confirmed.");
        myBody.GetComponent<Rigidbody>().isKinematic = true;
        myBody.transform.parent = playerCamera;
        beingCarried = true;
    }
}
