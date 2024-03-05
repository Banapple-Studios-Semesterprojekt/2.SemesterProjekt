using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerInput playerInput;

    [Header("References")]
    [SerializeField] private Transform cam;
    private CharacterController controller;

    [Header("Player Properties")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private float crouchSpeed = 1.5f;

    //Cached private variables
    private Vector3 move;
    private Vector3 fallVelocity;
    private float xRotation;
    private float currentSpeed;
    private float normalHeight;
    private float crouchHeight;
    private bool isCrouching;

    private void Awake()
    {
        //Creating new input
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    private void Start()
    {
        //Referencing objects
        controller = GetComponent<CharacterController>();

        currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        //Subscribing button events to functions
        playerInput.Player.Jump.performed += Jumping;
        playerInput.Player.Sprint.performed += Sprinting;
        playerInput.Player.Sprint.canceled += Sprinting;
        playerInput.Player.Crouch.performed += Crouching;
    }

    private void OnDisable()
    {
        //Unsubscribing button events to functions
        playerInput.Player.Jump.performed -= Jumping;
        playerInput.Player.Sprint.performed -= Sprinting;
        playerInput.Player.Sprint.canceled -= Sprinting;
        playerInput.Player.Crouch.performed += Crouching;
    }

    private void Sprinting(InputAction.CallbackContext context)
    {
        //Tunary operator that is an if-statement in setting the currentSpeed
        currentSpeed = context.performed ? runSpeed : walkSpeed;
    }

    private void Jumping(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            fallVelocity.y = jumpPower;
        }
    }

    private void Crouching(InputAction.CallbackContext context)
    {
        if (context.performed) //when controlkey is pressed
        {
            isCrouching = !isCrouching; //toggle
            controller.height = isCrouching ? 1 : 2;
            currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
            Debug.Log("Player is crouching");
        }
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCameraLook();
    }

    private void UpdateMovement()
    {
        //Getting player movement input
        Vector2 moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
        move = transform.forward * moveInput.y + transform.right * moveInput.x;

        //Resetting fallvelocity if grounded
        if (controller.isGrounded && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
        }

        //Calculating gravity
        fallVelocity.y += gravity * Time.deltaTime;

        //Actually moving
        controller.Move((move * currentSpeed + fallVelocity) * Time.deltaTime);
    }

    private void UpdateCameraLook()
    {
        //Getting mouse input
        Vector2 lookInput = playerInput.Player.CameraLook.ReadValue<Vector2>();
        //Setting xRotation to be the y-mouse movement
        xRotation -= lookInput.y * mouseSensitivity;
        //Limiting xRotation to be only between -90 and 90
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //Setting cameras xRotation
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Setting player bodys yRotation
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);
    }

    public Transform GetCamera()
    {
        return cam;
    }
}