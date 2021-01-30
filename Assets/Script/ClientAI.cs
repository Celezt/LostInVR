using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClientAI : MonoBehaviour
{
    public GameObject Player;
    public int DistanceThreshold = 3;


    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < DistanceThreshold)
            print("hej");
    }
}
