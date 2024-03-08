using UnityEngine;
using UnityEngine.InputSystem;


public class SwingNet : ItemMechanic
{
    private Animator animator;
    private CharacterController controller;
    private const int maxSpeed = 6;
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = transform.parent.GetComponentInParent<CharacterController>();
    }
    private void OnEnable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed += Swing;
    }

    private void OnDisable()
    {
        PlayerController.playerInput.Player.PrimaryAction.performed -= Swing;
    }

    private void Update()
    {
        animator.SetFloat("Velocity", controller.velocity.magnitude / maxSpeed, 0.125f, Time.deltaTime);
    }
    private void Swing(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Swing");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ItemObject>() && other.GetComponent<ItemObject>().item.itemType == ItemType.Butterfly)
        {
            other.GetComponent<ItemObject>().AddItemToInventoryWithNet();
        }
    }
}
