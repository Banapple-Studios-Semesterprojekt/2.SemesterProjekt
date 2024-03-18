using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private CharacterController controller;
    private Animator scientistAnimator;
    private float maxSpeed;

    private void Start()
    {
        scientistAnimator = transform.Find("Scientist").GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        maxSpeed = GetComponent<PlayerController>().GetRunSpeed();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        scientistAnimator.SetFloat("Velocity", controller.velocity.magnitude / maxSpeed, 0.125f, Time.deltaTime);
    }
}
