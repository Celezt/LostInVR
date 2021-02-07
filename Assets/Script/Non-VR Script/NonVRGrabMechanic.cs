using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NonVRGrabMechanic : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;
    public Transform ObjectHolder;
    [Tooltip("Can be empty.")]
    public GameObject Item;

    [Header("Grab Object")]
    [SerializeField]
    private string _holdingLayer;
    [SerializeField]
    private string _objectTag;
    public float RayLength;

    [Header("Throw Object")]
    public float ThrowForce;

    [Header("Audio (Optional)")]
    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private AudioClip[] _audioClipsGrab;
    [SerializeField]
    private AudioClip[] _audioClipsThrow;

    private Transform _itemPreviousParent;
    private Transform _itemTransform;
    private GameObject _itemColliderObject;
    private Collider _itemCollider;

    private int _itemColliderPreviousLayer;

    private bool _isCarrying;
    private bool _isThrowable;

    public bool IsCarrying { get => _isCarrying; }
    public bool IsThrowable { get => _isThrowable; }

    // Play random grab sound.
    public void PlayRandomAudioGrab()
    {
        if (!_audio || _audioClipsGrab == null)
            return;

        if (_audio.isPlaying)
            return;

        _audio.clip = _audioClipsGrab[Random.Range(0, _audioClipsGrab.Length)];
        _audio.Play();
    }

    // Play random throw sound.
    public void PlayRandomAudioThrow()
    {
        if (!_audio || _audioClipsThrow == null)
            return;

        if (_audio.isPlaying)
            return;

        _audio.clip = _audioClipsThrow[Random.Range(0, _audioClipsThrow.Length)];
        _audio.Play();
    }

    private void Update()
    {
        bool isGrabbing = Input.GetButtonDown("Grab");
        bool isThrowing = Input.GetButtonDown("Throw");

        if (isGrabbing && !_isCarrying)
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, RayLength))
            {
                _itemCollider = hit.collider;
                _itemColliderObject = _itemCollider.gameObject;

                if (_itemColliderObject.CompareTag(_objectTag))
                {
                    Rigidbody hitParentBody = _itemColliderObject.GetComponentInParent<Rigidbody>();

                    // Try first if the game object has a parent with RigidBody.
                    if (hitParentBody)
                        Item = hitParentBody.gameObject;
                    // Else try if the hit object has RigidBody.
                    else if (_itemColliderObject.GetComponent<Rigidbody>())
                        Item = _itemColliderObject;
                    else
                        return;

                    _itemTransform = Item.transform;

                    AttachItem();
                    AkSoundEngine.PostEvent("Item_Slide", gameObject);
                }
            }
        }
        else if (isThrowing && _isThrowable)
        {
            _isCarrying = false;
            _isThrowable = false;

            _itemTransform.parent = _itemPreviousParent;

            DetachItem(ThrowForce);
            PlayRandomAudioThrow();
        }

    }

    private void AttachItem()
    {
        _isCarrying = true;
        _isThrowable = true;

        _itemColliderPreviousLayer = _itemColliderObject.layer;
        _itemColliderObject.layer = LayerMask.NameToLayer(_holdingLayer);

        // Store object's parent before overriding it.
        _itemPreviousParent = _itemTransform.parent;
        _itemTransform.parent = ObjectHolder;

        _itemTransform.position = ObjectHolder.position;
        _itemTransform.localRotation = transform.rotation;

        Rigidbody itemBody = Item.GetComponent<Rigidbody>();
        itemBody.isKinematic = true;
        itemBody.useGravity = false;
    }

    private void DetachItem(float force = 0)
    {
        if (Item)
        {
            _itemColliderObject.layer = _itemColliderPreviousLayer;

            Rigidbody itemBody = Item.GetComponent<Rigidbody>();
            itemBody.isKinematic = false;
            itemBody.useGravity = true;
            itemBody.AddForce(_cameraTransform.forward * force);
        }

        _itemColliderPreviousLayer = 0;
        Item = null;
        _itemTransform = null;
        _itemPreviousParent = null;
        _itemCollider = null;
        _itemColliderObject = null;
    }
}
