using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindsTutorial : MonoBehaviour
{
    private Image[] moveLookImages;
    private TextMeshProUGUI[] moveLookText;

    [SerializeField] private float fadeDuration = 1f;

    private bool hasMoved = false;
    private bool hasLooked = false;
    private bool hasInventoried = false;
    private bool isFading;

    private void Start()
    {
        Transform t = transform.Find("Movement And Look");
        moveLookImages = t.GetComponentsInChildren<Image>();
        moveLookText = t.GetComponentsInChildren<TextMeshProUGUI>();

        PlayerController.playerInput.Player.Movement.performed += Movement_performed;
        PlayerController.playerInput.Player.CameraLook.performed += CameraLook_performed;
        PlayerController.playerInput.Player.Inventory.performed += Inventory_performed;
    }
    private void OnDestroy()
    {
        PlayerController.playerInput.Player.Movement.performed -= Movement_performed;
        PlayerController.playerInput.Player.CameraLook.performed -= CameraLook_performed;
        PlayerController.playerInput.Player.Inventory.performed -= Inventory_performed;
    }
    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        hasInventoried = true;
        CheckIfTutorialDone();
    }

    private void CameraLook_performed(InputAction.CallbackContext obj)
    {
        hasLooked = true;
        CheckIfTutorialDone();
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        hasMoved = true;
        CheckIfTutorialDone();
    }

    private void CheckIfTutorialDone()
    {
        if (hasMoved && hasLooked && hasInventoried && !isFading)
        {
            FadeGuide(moveLookImages, moveLookText, false);
        }
    }

    private void FadeGuide(Image[] images, TextMeshProUGUI[] texts, bool fadeIn)
    {
        isFading = true;

        foreach (Image image in images)
        {
            image.CrossFadeAlpha(fadeIn ? 1f : 0f, fadeDuration, true);
        }
        foreach (TextMeshProUGUI text in texts)
        {
            text.CrossFadeAlpha(fadeIn ? 1f : 0f, fadeDuration, true);
        }
    }
}