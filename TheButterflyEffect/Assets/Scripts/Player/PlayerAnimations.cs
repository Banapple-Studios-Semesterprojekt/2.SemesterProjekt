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
        GetComponent<PlayerController>().onCrouch += OnCrouch;
        transform.GetChildrenRecursive<SwingNet>(false).ToArray()[0].onSwing += OnSwing;
        Glowstick glowstickScript = transform.GetChildrenRecursive<Glowstick>(false).ToArray()[0];
        glowstickScript.onClick += OnClick;
        glowstickScript.onPoint += OnPoint;
        GetComponent<HeldItem>().onHoldItem += OnHoldItem;
    }

    private void OnHoldItem(string itemName)
    {
        scientistAnimator.SetBool("isNet", itemName == "Net");
        scientistAnimator.SetBool("isGlowstick", itemName == "Glowstick");
    }

    private void OnPoint(bool isPointing)
    {
        scientistAnimator.SetBool("isPointing", isPointing);
    }

    private void OnClick()
    {
        scientistAnimator.SetTrigger("Glowstick Click");
    }

    private void OnSwing()
    {
        scientistAnimator.SetTrigger("Swing Net");
    }

    private void OnCrouch(bool isCrouching)
    {
        scientistAnimator.SetBool("isCrouching", isCrouching);
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
