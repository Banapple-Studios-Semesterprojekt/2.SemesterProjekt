using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;

    [SerializeField] private int NoOfRays = 10;
    [SerializeField] private float visionAngle = 1;
    [SerializeField] private float visionRadius = 75;
    [SerializeField] private LayerMask rayMask;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerController.Instance().transform;
        animator = GetComponent<Animator>();

        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget()
    {
        while (target != null)
        {
            print("I have been called on");
            //Performs the function "CheckForPlayer()" which returns true or false, depending on this it will enter the if statement or not. 
            CheckForPlayer();

            //Current velocity / maximum speed = a number between 0 and 1. 
            //"Velocity" = Value in the animator that smoothly blends between the idle and running animation.
            animator.SetFloat("Velocity", agent.velocity.magnitude / agent.speed);

            yield return new WaitForSeconds(0.1f); //Delay to prevent Unity from crashing, it is impossible for "target" to become null in one frame.
        }
    }

    void CheckForPlayer()
    {
        for (int i = 0; i < NoOfRays; i++)
        {
            if(i == 0)
            {
                ShootRay(transform.forward);
            }
            float directionAngle = visionAngle / i;
            
            Vector3 directionRight = transform.forward + (transform.right * directionAngle); //Calculates raycast to the right.
            Vector3 directionLeft = transform.forward + (-transform.right * directionAngle); //"- transform.right" = transform to the left.

            //Raycast to the right and left and checks if "hit" is player or not.
            if (ShootRay(directionRight) || ShootRay(directionLeft))
            {
                agent.SetDestination(target.position); //Function that recalculates destination when target position changes.
            } 

        }
    }

    bool ShootRay(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f; //Starting point of raycast.

        //Draws raycasts so they can be visualized
        Debug.DrawRay(origin, direction * visionRadius, Color.red, 0.1f);

        if (Physics.Raycast(origin, direction, out hit, visionRadius, rayMask, QueryTriggerInteraction.Ignore))
        {
            print("hit = " + hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                print("I follow player :D");
                FollowTarget();
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            //Respawn function needs to be here. Function is probably in player script.
            //PlayerController.Instance(). <respawn-function-name> -- Thomas
            collider.GetComponent<Death_and_respawn>().Die();
            Debug.Log("Player dies");
        }
    }


}
