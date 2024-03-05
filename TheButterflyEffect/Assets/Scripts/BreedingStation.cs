using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreedingStation : MonoBehaviour
{
    public UnityEvent onInteract;

    [SerializeField] private GameObject inventoryCanvas;

    private bool isOpen = false;

    private void Start()
    {
        inventoryCanvas.SetActive(false);
    }

    public void OpenBreedingStation()
    {
        isOpen = !isOpen;
        inventoryCanvas.SetActive(isOpen);

        if (isOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /*// Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }*/
}