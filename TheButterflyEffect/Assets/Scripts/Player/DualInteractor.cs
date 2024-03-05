using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DualInteractor : MonoBehaviour
{
    private RaycastHit hit;
    private Transform cam;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 2f;

    [SerializeField] private LayerMask leftClickRayMask;
    [SerializeField] private LayerMask rightClickRayMask;

    private Interactable currentInteractable;

    private void Start()
    {
        cam = GetComponent<PlayerController>().GetCamera();
    }

    private void OnEnable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed += LeftClickInteract;
        PlayerController.playerInput.Player.SecondaryAction.performed += RightClickInteract;
    }

    private void OnDisable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed -= LeftClickInteract;
        PlayerController.playerInput.Player.SecondaryAction.performed -= RightClickInteract;
    }

    private void LeftClickInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable != null && context.control.IsPressed())
        {
            //currentInteractable.OnLeftClickInteract();
            currentInteractable.onInteract?.Invoke();
        }
    }

    private void RightClickInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable != null && context.control.IsPressed())
        {
            //currentInteractable.OnRightClickInteract();
            currentInteractable.onInteract?.Invoke();
        }
    }

    private void Update()
    {
        UpdateRaycast();
    }

    private void UpdateRaycast()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        LayerMask rayMask = GetRayMask();

        if (Physics.Raycast(ray, out hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore))
        {
            currentInteractable = hit.transform.GetComponent<Interactable>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    private LayerMask GetRayMask()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            return rightClickRayMask;
        }
        else
        {
            return leftClickRayMask;
        }
    }
}