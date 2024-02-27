using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StaminaController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;

    [Header("Stamina Main Parameters")]
    public float playerStamina = 100f; //Max stamina player has, measures how much stamina the player has left.
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float jumpCost = 20; //How much stamina it costs to jump. 
    //"HideInInspector" = Hidden in inspector but used if variables need to be public. 
    [HideInInspector] public bool hasRegenerated = true; //Stamina bar is regernerating?
    [HideInInspector] public bool sprinting; //Default is false

    [Header("Stamina Regen Parameters")]
    //"Range" = Restricts a variable to be between two values. In this case 0 and 50. 
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f; 
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;
    

    private void Awake()
    {
        sprinting = false;
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        PlayerController.playerInput.Player.Jump.performed += StaminaJump;
        PlayerController.playerInput.Player.Sprint.performed += Sprinting;
        PlayerController.playerInput.Player.Sprint.canceled += Sprinting;
    }


    private void Update()
    {
        if(!sprinting)
        {   //Regenerates stamina
            if(playerStamina <= maxStamina - 0.01)
            {   //Stamina regenerates over time.
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                playerController.SetCanRun(true);

                if (playerStamina >= maxStamina)
                {
                    playerStamina = maxStamina;
                    //Reset alpha value for slider
                    sliderCanvasGroup.alpha = 0;

                    hasRegenerated = true;
                }
            }
        }
        else
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerController.SetCanRun(false);
                sprinting = false;
            }
            
        }
    }

    public void Sprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hasRegenerated)
            {
                sprinting = true;

                if (playerStamina <= 0)
                {
                    hasRegenerated = false;
                    playerController.GetRunSpeed();
                    sliderCanvasGroup.alpha = 0; //Does not update stamina bar, but sets alpha to 0 --> Invisible.
                }
            }
        }
        else if (context.canceled)
        {
            sprinting = false;
        }
        
    }

    public void StaminaJump(InputAction.CallbackContext context)
    {   //If player stamina is greater than the jumpCost, jumpCost will be subtracted from playerStamina.
        if(playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            //Allow player to jump
            UpdateStamina(1); //Ensures the stamina bar's alpha is 1 --> Visible
        }
    }

    void UpdateStamina (int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina; //playerStamina will always be less than maxStamine, will result in a decimal number between 0 and 1.

        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
