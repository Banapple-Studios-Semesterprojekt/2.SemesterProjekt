using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq.Expressions;

public class StaminaController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    private CharacterController playerCharacterController;

    [Header("Stamina Main Parameters")]
    public float playerStamina = 100f; //Max stamina player has, measures how much stamina the player has left.
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float jumpCost = 20; //How much stamina it costs to jump. 
    //"HideInInspector" = Hidden in inspector but used if variables need to be public. 
    public bool sprinting;
    public bool isMoving;
    public bool regenBegun;
    private bool canRegen;


    [Header("Stamina Regen Parameters")]
    //"Range" = Restricts a variable to be between two values. In this case 0 and 50. 
    [Range(0, 50)][SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.5f;
    [SerializeField] private float delayStaminaRegen = 3f;

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

        playerCharacterController = GetComponent<CharacterController>(); 
    }


    private void Update()
    {
        //SETS isMoving to "true" when player's velocity is greater than 0, if not it is SET to "false".
        isMoving = playerCharacterController.velocity.sqrMagnitude > 0;

        if (!sprinting || (sprinting && !isMoving))
        {
            if (playerStamina <= maxStamina && canRegen)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                playerController.SetCanRun(true);

                UpdateStamina(1);
            }

            //Removes stamina bar, so it is not visible.
            if (playerStamina >= maxStamina )
            {
                playerStamina = maxStamina;
                //Reset alpha value for slider
                UpdateStamina(0);
            }

        }

        else if (sprinting && isMoving)
        {  
            if(regenBegun)
            {
                CancelInvoke();
                canRegen = false;
                regenBegun = false;
            }

            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerStamina = 0;
                playerController.SetCanRun(false);
            }
        }

        //Can still regen when you are not moving and pressing Lshift
        if((sprinting && !isMoving && !regenBegun) || (!sprinting && !regenBegun))
        {
            canRegen = false;
            CancelInvoke();
            Invoke("DelayStaminaBarRegen", delayStaminaRegen);
            regenBegun = true;
        }

    }

    public void DelayStaminaBarRegen()
    {
        canRegen = true;
        Debug.Log("Im confused");
    }

    public void Sprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //CancelInvoke();
            sprinting = true;
            
        }
        else if (context.canceled)
        {
            sprinting = false;
            /*canRegen = false;
            Invoke("DelayStaminaBarRegen", delayStaminaRegen);
            regenBegun = false;*/
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
