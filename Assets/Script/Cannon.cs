using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public MeshController meshControl;
    public float POWER = 500f;
    public int itemsPerShot = 10;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < itemsPerShot; i++)
        {
            GameObject newObject = meshControl.NewObject(transform);
            newObject.GetComponent<Rigidbody>().AddForce(transform.up * POWER);
        }
    }
}
