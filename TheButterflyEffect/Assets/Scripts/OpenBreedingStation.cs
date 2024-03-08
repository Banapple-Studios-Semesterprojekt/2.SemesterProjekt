using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This class handles the opening and closing of the breeding station UI.
/// </summary>
public class OpenBreedingStation : MonoBehaviour
{
    /// <summary>
    /// Event triggered when the breeding station is interacted with.
    /// </summary>
    public UnityEvent onInteract;

    private PlayerController player;

    /// <summary>
    /// The UI canvas for the breeding station.
    /// </summary>
    [SerializeField] private GameObject breedingCanvas;

    private bool isOpen = false;

    /// <summary>
    /// Initializes the breeding station UI.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        breedingCanvas.SetActive(false);
    }

    /// <summary>
    /// Toggles the visibility of the breeding station UI.
    /// </summary>
    public void OpenBS()
    {
        isOpen = !isOpen;
        breedingCanvas.SetActive(isOpen);
        if (player != null)
        {
            player.enabled = !isOpen;
        }
        else
        {
            Debug.LogError("PlayerController component not found on this game object.");
        }
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Checks if the Exit action is triggered and the breeding station is open, and if so, closes the breeding station.
    /// </summary>
    private void Update()
    {
        if (PlayerController.playerInput.Player.Exit.triggered && isOpen)
        {
            CloseBreedingStation();
        }
    }

    /// <summary>
    /// Closes the breeding station UI.
    /// </summary>
    public void CloseBreedingStation()
    {
        isOpen = false;
        breedingCanvas.SetActive(isOpen);
        if (player != null)
        {
            player.enabled = true;
        }
        else
        {
            Debug.LogError("PlayerController component not found on this game object.");
        }
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}