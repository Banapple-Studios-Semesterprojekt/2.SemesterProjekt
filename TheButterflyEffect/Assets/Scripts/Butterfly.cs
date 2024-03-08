using UnityEngine;

public class Butterfly : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float minFlyHeight = 1f;
    public float maxFlyHeight = 3f;
    public Vector2 areaSize = new Vector2(10f, 10f); // Size of the area where butterflies can move

    private Vector3 targetPosition;
    private float changeDirectionTimer;

    private void Start()
    {
        SetRandomTargetPosition();
    }

    private void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }

        // Change direction timer
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            SetRandomTargetPosition();
            changeDirectionTimer = changeDirectionInterval;
        }

        // Adjust height
        float newY = Mathf.PingPong(Time.time * 0.5f, maxFlyHeight - minFlyHeight) + minFlyHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Ensure butterfly stays within the area
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -areaSize.x / 2f, areaSize.x / 2f),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -areaSize.y / 2f, areaSize.y / 2f)
        );
    }

    private void SetRandomTargetPosition()
    {
        // Generate a random target position within the specified range
        float randomX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float randomZ = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }
}
