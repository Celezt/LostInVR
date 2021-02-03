using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class NonVRMovement : MonoBehaviour
{
    [Tooltip("The Camera the player looks through.")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]

    public float Speed = 10f;
    public float JumpForce = 1000f;
    [Tooltip("Added gravity force to prevent floatiness.")]
    public float GravityForce = 3000f;
    public float LookSensitivity = 120f;

    [Header("Ground Check")]
    [SerializeField]
    private Transform _sphereTransform;
    public float Radius = 0.6f;
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
    private bool _isJump;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        // Player movement.
        float moveHorizontal    = Input.GetAxisRaw("Horizontal");
        float moveVertical      = Input.GetAxisRaw("Vertical");
        // If pressed jump.
        _isJump                 = Input.GetButtonDown("Jump");
        // Mouse position.
        float cameraHorizontal  = Input.GetAxisRaw("Mouse X");
        float cameraVertical    = Input.GetAxisRaw("Mouse Y");

        // Normalized direction.
        _direction = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        _velocity = _direction * Speed;

        _deltaRotation = new Vector3(0, cameraHorizontal, 0) * LookSensitivity * Time.deltaTime;
        _cameraDeltaRotation = new Vector3(cameraVertical, 0, 0) * LookSensitivity * Time.deltaTime;
        _cameraRotation -= _cameraDeltaRotation;

        // Jump if on ground.
        if (_isJump && _isGrounded)
            _body.AddForce(transform.up * JumpForce);

        InternalLockUpdate();
    }

    public void FixedUpdate()
    {
        // Clamp vertical rotation.
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90f, 90f);

        if (_camera != null)
            _camera.transform.localRotation = Quaternion.Euler(_cameraRotation);

        // Check with a sphere if the player is colliding with the ground.
        if (Physics.CheckSphere(_sphereTransform.position, Radius, GroundLayer.value))
            _isGrounded = true;
        else
            _isGrounded = false;

        // Add down force when velocity y is negative.
         if (!_isGrounded && _body.velocity.y < 0)
            _body.AddForce(-transform.up * GravityForce);

        // Move the player.
        if (_velocity != Vector3.zero)
            _body.MovePosition(_body.position + _velocity * Time.fixedDeltaTime);

        // Rotate the player and camera (horizontally).
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
