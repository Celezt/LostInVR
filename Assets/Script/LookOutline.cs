using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookOutline : MonoBehaviour
{
    [SerializeField]
    private Transform _castFrom;
    public float RayLength = 100f;

    private GameObject _targetedObject;
    private Outline _targetOutline;

    private void FixedUpdate()
    {
        ObjectOutline();
    }

    private void ObjectOutline()
    {
        if (Physics.Raycast(_castFrom.position, _castFrom.forward, out RaycastHit hit, RayLength))
        {
            GameObject _hitObject = hit.collider.gameObject;

            if (_targetedObject == _hitObject)
            {
                _targetOutline.enabled = true;
            }
            else
            {
                _targetedObject = _hitObject;

                if (_targetOutline)
                    _targetOutline.enabled = false;

                _targetOutline = _hitObject.GetComponent<Outline>();
            }
        }
    }
}
