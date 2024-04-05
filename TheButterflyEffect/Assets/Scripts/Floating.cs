using UnityEngine;

public class Floating : MonoBehaviour
{
    // Adjust to control the floating speed
    [SerializeField] private float floatSpeed = 1.0f;

    // Minimum and maximum values for random floating height
    [SerializeField] private float minFloatHeight = 0.2f;
    [SerializeField] private float maxFloatHeight = 1.0f;

    private Vector3 initialPosition;
    private float floatHeight;

    private float randomStart;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the initial position to the object's transform position
        initialPosition = transform.position;

        // Generate a random floating height within the specified range
        floatHeight = Random.Range(minFloatHeight, maxFloatHeight);

        randomStart = Random.Range(0, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the float offset based on Perlin noise
        float floatOffset = Mathf.PerlinNoise((Time.time + randomStart) * floatSpeed, 0) * 2 - 1;

        // Apply the float offset to the initial position
        Vector3 newPosition = initialPosition + Vector3.up * floatOffset * floatHeight;

        // Update the object's position
        transform.position = newPosition;
    }

    private void OnEnable()
    {
        initialPosition = transform.position;
    }
}
