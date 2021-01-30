using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class LookOutline : MonoBehaviour
{
    private Outline _outline;
    private Renderer _renderer;

    public float DistanceThreshold = 3f;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        ObjectOutline();
    }

    private void ObjectOutline()
    {
        _outline.enabled = false;

        if (_renderer.isVisible)
            if (Vector3.Distance(Camera.current.transform.position, transform.position) < DistanceThreshold)
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.current.transform.position, Camera.current.transform.forward, out hit, int.MaxValue))
                {
                    if (hit.collider.gameObject == gameObject)
                        _outline.enabled = true;
                }
            }
    }
}
