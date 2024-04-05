using UnityEngine;

/// <summary>
/// This class handles the opening and closing of the breeding station UI.
/// </summary>
public class OpenBreedingStation : MonoBehaviour
{
    /// <summary>
    /// The UI canvas for the breeding station.
    /// </summary>
    [SerializeField] private GameObject breedingCanvas;

    /// <summary>
    /// Initializes the breeding station UI.
    /// </summary>
    private void Start()
    {
        breedingCanvas.SetActive(false);
    }

    /// <summary>
    /// Toggles the visibility of the breeding station UI.
    /// </summary>
    public void OpenBS()
    {
        breedingCanvas.SetActive(true);
        Inventory.Instance().SetInventory(true);
    }

    /// <summary>
    /// Closes the breeding station UI.
    /// </summary>
    public void CloseBS()
    {
        breedingCanvas.SetActive(false);
    }
}