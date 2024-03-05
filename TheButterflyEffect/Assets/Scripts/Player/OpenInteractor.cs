using UnityEngine;
using UnityEngine.InputSystem;

public class OpenInteractor : MonoBehaviour
{
    private RaycastHit hit;
    private Transform cam;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 2f;

    [SerializeField] private LayerMask rayMask;

    private BreedingStation currentInteractable;

    private void Start()
    {
        cam = GetComponent<PlayerController>().GetCamera();
        print("Start");
        PlayerController.playerInput.Player.SecondaryAction.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            print("interact");
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
        if (Physics.Raycast(ray, out hit, rayDistance, rayMask, QueryTriggerInteraction.Ignore) && hit.transform.GetComponent<BreedingStation>())
        {
            print("if raycast");
            currentInteractable = hit.transform.GetComponent<BreedingStation>();
        }
        else
        {
            print("else");
            currentInteractable = null;
        }
    }
}