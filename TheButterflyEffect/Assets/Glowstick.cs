using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowstick : ItemMechanic
{
    //Bug fixes:
    //When right clicking and then turning on glowstick --> Turns on both lights.
   
    private Animator animator;
    private CharacterController characterController;
    private const float maxSpeed = 6;

    [SerializeField] private Light pointlight;
    [SerializeField] private Light spotlight;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //Getting character controller from parent of glowstick --> main camera --> player
        characterController = transform.parent.GetComponentInParent<CharacterController>();

        
    }

    private void Update()
    {
        //calculates velocity, by dividing velocity by "maxSpeed", the value can only be between 0 and 1. 
        float currentVelocity = characterController.velocity.magnitude / maxSpeed;
        //Finds "GlowstickWalking" layer in animator and sets "weight" (parameter in animator, blend tree) to the currentVelocity.
        animator.SetLayerWeight(1, currentVelocity);
    }

    private void Spotlight()
    {   //Turnary operator
        //Sets bool of isPointing to the opposite of what it currently is.
        animator.SetFloat("State", animator.GetFloat("State") == 0 ? 1 : 0);
        if(pointlight.enabled || spotlight.enabled)
        {   //Only enables light if turned on. 
            pointlight.enabled = animator.GetFloat("State") == 0;
            spotlight.enabled = animator.GetFloat("State") == 1;
        }
        
    }

    private void TurnOn()
    {
        //Checks if glowstick is turned on, and turns it on/off accordingly.
        if(pointlight.enabled || spotlight.enabled)
        {
            pointlight.enabled = false;
            spotlight.enabled = false;
        }
        else
        {
            pointlight.enabled = true;
            spotlight.enabled = true;
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
