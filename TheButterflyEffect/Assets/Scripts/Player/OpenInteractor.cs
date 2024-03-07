using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class handles the interaction with the OpenBreedingStation object.
/// </summary>
public class OpenInteractor : MonoBehaviour
{
    private RaycastHit hit;
    private Transform cam;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 2f;

    [SerializeField] private LayerMask rayMask;

    /// <summary>
    /// The current OpenBreedingStation object that the player is interacting with.
    /// </summary>
    private OpenBreedingStation currentInteractable;

    /// <summary>
    /// Initializes the OpenInteractor by getting the camera and subscribing to the SecondaryAction event.
    /// </summary>
    private void Start()
    {
        cam = GetComponent<PlayerController>().GetCamera();
        PlayerController.playerInput.Player.SecondaryAction.performed += Interact;
    }

    /// <summary>
    /// Invokes the onInteract event of the current OpenBreedingStation object when the SecondaryAction event is performed.
    /// </summary>
    private void Interact(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.onInteract?.Invoke();
        }
    }

    /// <summary>
    /// Updates the raycast to check for interaction with an OpenBreedingStation object.
    /// </summary>
    private void Update()
    {
        UpdateRaycast();
    }

    /// <summary>
    /// Performs a raycast to check for interaction with an OpenBreedingStation object.
    /// </summary>
    private void UpdateRaycast()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(ray, out hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore) && hit.transform.GetComponent<OpenBreedingStation>())
        {
            currentInteractable = hit.transform.GetComponent<OpenBreedingStation>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    /// <summary>
    /// Unsubscribes from the SecondaryAction event when the OpenInteractor is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        PlayerController.playerInput.Player.SecondaryAction.performed -= Interact;
    }
}