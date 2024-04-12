using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;
    private AudioSource sound;
    private AudioClip howl, grunt, bark;

    private Transform[] patrolPositions;
    private bool isPatrolling;
    private Coroutine patrolCoroutine;

    [Header("Patrolling Properties")]
    [SerializeField] private float patrolTime = 15f;

    [Header("Raycast Cone Properties")]
    [SerializeField] private int NoOfRays = 10;
    [SerializeField] private float visionAngle = 1;
    [SerializeField] private float visionRadius = 75;
    [SerializeField] private LayerMask rayMask;

    private bool howlDone = false;
    [Header("Check Sphere Properties")]
    [SerializeField] private float sphereRadius = 4f;
    [SerializeField] private LayerMask sphereMask;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerController.Instance().transform;
        animator = GetComponent<Animator>();

        sound = GetComponent<AudioSource>();
        AudioData data = AudioManager.Instance().data;
        howl = data.howl;
        grunt = data.grunt;
        bark = data.bark;

        PlayAudio(howl, false, 0.95f);
        Invoke(nameof(SetHowlDone), 7f);
        
        GameObject patrolPointsParent = GameObject.Find("Wolf Patrol Positions");
        patrolPositions = patrolPointsParent.GetComponentsInChildren<Transform>().Where(x => x != patrolPointsParent.transform).ToArray();

        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget()
    {
        while (target != null)
        {
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
        //Raycasting Cone Vision
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
                isPatrolling = false;
                return;
            } 

        if(agent.velocity.magnitude > 0 && howlDone)
        {
            PlayAudio(bark, true, 0.75f);
        }
        else if(howlDone)
        {
            PlayAudio(grunt, true, 1f);
        }

        //Sphere cast: checking for if player is nearby
        if (Physics.CheckSphere(transform.position, sphereRadius, sphereMask, QueryTriggerInteraction.Ignore))
        {
            agent.SetDestination(target.position);
            isPatrolling = false;
            return;
        }

        if(patrolCoroutine == null)
        {
            isPatrolling = true;
            patrolCoroutine = StartCoroutine(Patrol());
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

    IEnumerator Patrol()
    {
        while(isPatrolling)
        {
            agent.SetDestination ( patrolPositions[Random.Range(0, patrolPositions.Length)].position );
            yield return new WaitForSeconds(Random.Range(patrolTime / 1.5f, patrolTime));
        }
        patrolCoroutine = null;
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

    private void PlayAudio(AudioClip clip, bool loop, float pitch)
    {
        if(sound.clip != clip)
        {
             sound.clip = clip;
             sound.loop = loop;
             sound.pitch = pitch;
             sound.Play();
        }
    }

    private void SetHowlDone()
    {
        howlDone = true;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255f, 255, 0f, 100f / 255f);
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
