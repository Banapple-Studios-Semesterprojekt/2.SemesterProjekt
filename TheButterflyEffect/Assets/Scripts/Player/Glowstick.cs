using UnityEngine;

public class Glowstick : ItemMechanic
{
    //Bug fixes:
    //When right clicking and then turning on glowstick --> Turns on both lights.
    private Material glowstickMat;
    private Color litColor;
    private const float maxSpeed = 6;

    [SerializeField] private Light pointlight;
    [SerializeField] private Light spotlight;
    [SerializeField] private bool startOn = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        glowstickMat = GetComponent<Renderer>().material;
        litColor = glowstickMat.GetColor("_EmissionColor");

        pointlight.enabled = startOn;
        spotlight.enabled = startOn;

        glowstickMat.SetColor("_EmissionColor", startOn ? litColor : Color.black);
    }

    private void Update()
    {
        //calculates velocity, by dividing velocity by "maxSpeed", the value can only be between 0 and 1. 
        float currentVelocity = controller.velocity.magnitude / maxSpeed;
        //Finds "GlowstickWalking" layer in animator and sets "weight" (parameter in animator, blend tree) to the currentVelocity.

        animator.SetFloat("Velocity", currentVelocity, 0.125f, Time.deltaTime);
    }

    private void Spotlight()
    {
        //Turnary operator
        //Sets bool of isPointing to the opposite of what it currently is.
        animator.SetBool("isPointing", !animator.GetBool("isPointing"));
        bool points = animator.GetBool("isPointing");
        if (pointlight.enabled || spotlight.enabled)
        {   //Only enables light if turned on. 
            pointlight.enabled = !points;
            spotlight.enabled = points;
            glowstickMat.SetColor("_EmissionColor", points ? Color.black : litColor);
        }  
    }

    private void TurnOn()
    {
        //Checks if glowstick is turned on, and turns it on/off accordingly.
        if(pointlight.enabled || spotlight.enabled)
        {
            pointlight.enabled = false;
            spotlight.enabled = false;
            glowstickMat.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            bool points = animator.GetBool("isPointing");
            pointlight.enabled = !points;
            spotlight.enabled = points;
            glowstickMat.SetColor("_EmissionColor", points ? Color.black : litColor);
        }

    }

    private void OnEnable()
    {
        heldItemScript.onPrimaryAction += TurnOn;
        heldItemScript.onSecondaryAction += Spotlight;

    }
    private void OnDisable()
    {
        heldItemScript.onPrimaryAction -= TurnOn;
        heldItemScript.onSecondaryAction -= Spotlight;
    }
}
