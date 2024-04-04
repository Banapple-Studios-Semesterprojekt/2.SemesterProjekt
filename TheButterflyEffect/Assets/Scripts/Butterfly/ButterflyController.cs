using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public ButterflyData butterflyData; // Reference to the ButterflyData Scriptable Object

    private void Start()
    {
        // Spawn the butterfly
        SpawnButterfly();
    }

    private void SpawnButterfly()
    {
        // Check if ButterflyData is assigned
        if (butterflyData == null)
        {
            Debug.LogError("ButterflyData is not assigned in the ButterflyController!");
            return;
        }

        // Instantiate the butterfly model
        GameObject butterflyModel = Instantiate(butterflyData.modelPrefab, transform.position, Quaternion.identity);

        // Set the parent of the butterfly model to the controller's GameObject
        butterflyModel.transform.parent = transform;

        // Access attributes from the ButterflyData Scriptable Object
        string name = butterflyData.butterflyName;
        string description = butterflyData.description;

        // Perform actions based on butterfly attributes
        Debug.Log("Spawned a " + name + " butterfly");

        // You can add more logic here based on the butterfly attributes
    }
}