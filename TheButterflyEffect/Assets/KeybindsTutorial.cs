using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindsTutorial : MonoBehaviour
{
    private Image[] moveLookImages;
    private TextMeshProUGUI moveLooktext;

    [SerializeField] private float fadeDuration = 1f;

    private bool hasMoved = false;
    private bool hasLooked = false;
    private bool isFading;

    private void Start()
    {
        Transform t = transform.Find("Movement And Look");
        moveLookImages = t.GetComponentsInChildren<Image>();
        moveLooktext = t.GetComponentInChildren<TextMeshProUGUI>();

        PlayerController.playerInput.Player.Movement.performed += Movement_performed;
        PlayerController.playerInput.Player.CameraLook.performed += CameraLook_performed;
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
        if(hasMoved && hasLooked && !isFading)
        {
            FadeGuide(moveLookImages, moveLooktext, false);
        }
    }

    private void FadeGuide(Image[] images, TextMeshProUGUI text, bool fadeIn)
    {
        isFading = true;

        foreach (Image image in images)
        {
            image.CrossFadeAlpha(fadeIn ? 1f : 0f, fadeDuration, true);
        }
        text.CrossFadeAlpha(fadeIn ? 1f : 0f, fadeDuration, true);
    }
}