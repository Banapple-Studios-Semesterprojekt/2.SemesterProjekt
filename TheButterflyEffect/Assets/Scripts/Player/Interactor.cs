using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private RaycastHit hit;
    private Transform cam;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 2f;

    [SerializeField] private LayerMask rayMask;

    private Interactable currentInteractable;

    private void Start()
    {
        cam = GetComponent<PlayerController>().GetCamera();
        PlayerController.playerInput.Player.PrimaryAction.performed += Interact;
        PlayerController.playerInput.Player.RightClick.performed += Inventory;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.onInteract?.Invoke();
        }
    }

    private void Inventory(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.gameObject.CompareTag("Inventory"))
            {
                currentInteractable.onInventory?.Invoke();
            }
        }
    }

    private void Update()
    {
        UpdateRaycast();
    }

    private void UpdateRaycast()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(ray, out hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore) && hit.transform.GetComponent<Interactable>())
        {
            currentInteractable = hit.transform.GetComponent<Interactable>();
        }
        else
        {
            currentInteractable = null;
        }
    }
}