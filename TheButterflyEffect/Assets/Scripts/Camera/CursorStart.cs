using UnityEngine;

public class CursorStart : MonoBehaviour
{
    [SerializeField] private bool isVisible = false;

    private void Start()
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
