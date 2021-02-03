using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class NonVRMovement : MonoBehaviour
{
    [Tooltip("The Camera the player looks through.")]
    [SerializeField]
    private Camera _camera;

    public float Speed = 10f;
    public float JumpForce = 1000f;
    public float LookSensitivity = 100f;

    [Header("Ground Check")]
    public float RayLenght = 1.2f;
    [SerializeField]
    private LayerMask GroundLayer;

    private Vector3 _direction;
    private Vector3 _velocity;
    private Vector3 _deltaRotation;
    private Vector3 _cameraDeltaRotation;
    private Vector3 _cameraRotation;

    private Rigidbody _body;

    private bool _isCursorLocked = true;
    private bool _isGrounded = true;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        float moveHorizontal    = Input.GetAxis("Horizontal");
        float moveVertical      = Input.GetAxis("Vertical");
        bool isJump             = Input.GetButtonDown("Jump");

        // Normalized direction.
        _direction = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        _velocity = _direction * Speed;

        // Mouse movement.
        float rotationHorizontal = Input.GetAxisRaw("Mouse X");
        float rotationVertical = Input.GetAxisRaw("Mouse Y");

        _deltaRotation = new Vector3(0, rotationHorizontal, 0) * LookSensitivity * Time.deltaTime;
        _cameraDeltaRotation = new Vector3(rotationVertical, 0, 0) * LookSensitivity * Time.deltaTime;
        _cameraRotation -= _cameraDeltaRotation;

        // Jump if on ground.
        if (isJump && _isGrounded)
            _body.AddForce(transform.up * JumpForce);

        // Shoot ray downwards to check if there is ground.
        if (Physics.Raycast(transform.position, -transform.up, out _, RayLenght, GroundLayer))
            _isGrounded = true;
        else
            _isGrounded = false;

        // Clamp vertical rotation.
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90f, 90f);

        if (_camera != null)
            _camera.transform.localRotation = Quaternion.Euler(_cameraRotation);

        InternalLockUpdate();
    }

    public void FixedUpdate()
    {
        // Move the player.
        if (_velocity != Vector3.zero)
            _body.MovePosition(_body.position + _velocity * Time.fixedDeltaTime);

        // Rotate the camera of the player.
        if (_deltaRotation != Vector3.zero)
            _body.MoveRotation(_body.rotation * Quaternion.Euler(_deltaRotation));
    }

    // Controls the locking and unlocking of the mouse.
    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            _isCursorLocked = false;
        else if (Input.GetMouseButtonUp(0))
            _isCursorLocked = true;

        if (_isCursorLocked)
            UnlockCursor();
        else if (!_isCursorLocked)
            LockCursor();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
