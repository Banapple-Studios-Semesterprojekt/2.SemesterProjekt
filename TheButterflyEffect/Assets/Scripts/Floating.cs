using UnityEngine;

public class Floating : MonoBehaviour
{
    //Adjust to control the floating speed
    [SerializeField] private float floatSpeed = 1.0f;
    //Adjust to control the floating height
    [SerializeField] private float floatHeight = 0.5f;

    private Vector3 initialPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        //Setting the initial position to the objects transform position
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculating the new Y position based on a sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
