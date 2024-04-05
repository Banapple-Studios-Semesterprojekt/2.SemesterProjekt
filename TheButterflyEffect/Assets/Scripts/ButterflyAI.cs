using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButterflyAI : MonoBehaviour
{
   [SerializeField] private float fleeSpeed = 2;
   [SerializeField] private float fleeRadius = 7;
    private float Updatespeed;
    private float updateIdle = 1f;
    private float updateAlert = 0.25f;
    private bool flee;
    private GameObject Player;
    private CharacterController controller;

    //Floating Variables ----->
    [Space]

    // Adjust to control the floating speed
    [SerializeField] private float floatSpeed = 1.0f;

    // Minimum and maximum values for random floating height
    [SerializeField] private float minFloatHeight = 0.2f;
    [SerializeField] private float maxFloatHeight = 1.0f;

    private Vector3 initialPosition;
    private float floatHeight;

    private float randomStart;

    private void Start()
    {
        FloatingSetup();
        FleeSetup();
    }

    private void Update()
    {
        if(flee)
        {
            UpdateFlee();
        }
        else
        {
            UpdateFloating();
        }
    }

    private void UpdateFlee()
    {
        Vector3 v3 = new Vector3(transform.position.x - Player.transform.position.x, 0, transform.position.z - Player.transform.position.z).normalized;
        transform.position += v3 * fleeSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v3), 10f * Time.deltaTime);
    }

    private void UpdateFloating()
    {
        // Calculate the float offset based on Perlin noise
        float floatOffset = Mathf.PerlinNoise((Time.time + randomStart) * floatSpeed, 0) * 2 - 1;

        // Apply the float offset to the initial position
        Vector3 newPosition = initialPosition + Vector3.up * floatOffset * floatHeight;

        // Update the object's position
        transform.position = newPosition;
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            
            if (FindDistanceToPlayer() < fleeRadius)
            {
                if (controller.height == 2)
                {
                    Updatespeed = updateAlert;
                    flee = true;
                }
                
            }
            else
            {
                Updatespeed = updateIdle;
                initialPosition = transform.position;
                flee = false;
            }
            yield return new WaitForSeconds(Updatespeed);
        }
      
    }

    private float FindDistanceToPlayer()
    {
        return Mathf.Sqrt( Mathf.Pow(Player.transform.position.x - transform.position.x, 2) + Mathf.Pow(Player.transform.position.z - transform.position.z, 2));
    }

    private void FleeSetup()
    {
        Updatespeed = updateIdle;
        Player = GameObject.Find("Player");
        controller = Player.GetComponent<CharacterController>();
        StartCoroutine(DetectPlayer());
    }

    private void FloatingSetup()
    {
        // Setting the initial position to the object's transform position
        initialPosition = transform.position;

        // Generate a random floating height within the specified range
        floatHeight = Random.Range(minFloatHeight, maxFloatHeight);

        randomStart = Random.Range(0, 1000);
    }

}
       
    

