using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public UnityEvent onInventory;

    private void Start()
    {
        // Tilknyt input til Interact-metoden
        PlayerController.playerInput.Player.PrimaryAction.performed += LeftClick;
        PlayerController.playerInput.Player.RightClick.performed += RightClick;
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left click");
        Destroy(gameObject);
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Right click");
    }
}