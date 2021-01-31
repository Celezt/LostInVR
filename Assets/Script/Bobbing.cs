using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    Vector3 _y;
    float _offset;

    public float BobStrength = 1;
    public float BobSpeed = 1;

    void Start()
    {
        _offset = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            _offset + (Mathf.Sin(Time.time * BobSpeed) * BobStrength), transform.position.z);
    }
}
