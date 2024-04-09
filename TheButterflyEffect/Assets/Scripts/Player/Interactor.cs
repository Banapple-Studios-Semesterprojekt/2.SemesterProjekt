using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Interactor : MonoBehaviour
{
    private Transform cam;
    [SerializeField] private Image interactIcon;
    [SerializeField] private float i_Distance = 2f;
    [SerializeField] private LayerMask i_Mask;
    [SerializeField] private Vector2 normalSize = Vector2.one * 20, highlightSize = Vector2.one * 30;
    [SerializeField] private float highlightSmooth = 10f;
    public bool canRaycast = true;

    private RaycastHit hit;
    private Ray ray;
    private bool hitInteractable = false;

    private Interactable interactable;

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        Interact();
    }

    void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            cam = playerController.GetCamera();
        }
        StartCoroutine(SetInteractIcon(false));
    }

    private void Update()
    {
        UpdateInteractor();
    }

    private void UpdateInteractor()
    {
        ray = new(cam.position, cam.forward);
        if (Physics.Raycast(ray, out hit, i_Distance, i_Mask, QueryTriggerInteraction.Ignore) && canRaycast)
        {
            bool canInteract = hit.transform.TryGetComponent(out interactable);
            if (canInteract)
            {
                if (!hitInteractable)
                {
                    StopAllCoroutines();
                    StartCoroutine(SetInteractIcon(true));
                    hitInteractable = true;
                }
            }
        }
        else
        {
            interactable = null;
            if (hitInteractable)
            {
                StopAllCoroutines();
                StartCoroutine(SetInteractIcon(false));
                hitInteractable = false;
                Debug.Log("Not hitting interactable");
            }
        }
    }

    private void Interact()
    {
        if (interactable != null)
        {
            interactable.onInteract?.Invoke();
        }
    }

    private IEnumerator SetInteractIcon(bool isVisible)
    {
        RectTransform rect = interactIcon.GetComponent<RectTransform>();
        Vector2 desiredSize = isVisible ? highlightSize : normalSize;
        Color desiredColor = isVisible ? new Color(1, 1, 1, 1f) : new Color(1f, 1f, 1f, 10f / 255f);
        while (Mathf.Abs(rect.sizeDelta.x - desiredSize.x) > 0.01f)
        {
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, desiredSize, highlightSmooth * Time.deltaTime);
            interactIcon.color = Color.Lerp(interactIcon.color, desiredColor, highlightSmooth * Time.deltaTime);
            yield return null;
        }

        rect.sizeDelta = desiredSize;
        interactIcon.color = desiredColor;
    }

    private void OnEnable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed += Interact_performed;
    }


    private void OnDisable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed -= Interact_performed;
    }
}