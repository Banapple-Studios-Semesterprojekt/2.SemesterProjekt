using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSadFace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    private CharacterController playerCharacterController;

    [Header("Stamina parameters")]
    public float playerStamina = 100;
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float jumpCost = 20;
    public bool isMoving;
    public bool sprinting;
    public bool canJump;

    [Header("Stamina regen parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain;
    [Range(0, 50)][SerializeField] private float staminaRegen;
    public bool isRegenerating;
    public float regenTime = 3f;

    [Header("Stamina UI")]
    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;

    private Coroutine StaminaCoroutine;
    private Coroutine DelayRegenCoroutine;

    private void Start()
    {
        //Get components to references
        playerController = GetComponent<PlayerController>();
        playerCharacterController = GetComponent<CharacterController>();

        //Sets the Stamina() coroutine as the variable "StaminaCoroutine".
        StaminaCoroutine = StartCoroutine(Stamina());
        canJump = true; //Ensures canJump is true from the start of the game.
    }

    private void Update()
    {
        //This SETS the bools "isMoving" and "isRegenerating" to TRUE when the condition(s) on the righthand side of the equality sign are satisfied. 
        //The bools are false in all other cases.
        isMoving = playerCharacterController.velocity.sqrMagnitude > 0;
        isRegenerating = (!sprinting ^ (sprinting && !isMoving)) && DelayRegenCoroutine == null;

    }
   
    //Function that is called when the player jumps ("onJump").
    private void Jumping()
    {
        //Substracts the jump cost from the player's stamina.
        playerStamina -= jumpCost;
        UpdateStaminaUI(1);

        //If the DelayRegenCoroutine is not null, then it will be stopped. Else it is started.
        if (DelayRegenCoroutine != null) { StopCoroutine(DelayRegenCoroutine); }
        DelayRegenCoroutine = StartCoroutine(DelayRegenerating());

        //Calls function from player-controller and sets the bool "canJump" to true. 
        playerController.SetCanJump(true);
        canJump = true;
        //if the "playerStamina" is less than jumpCost, the player cannot jump. 
        if (playerStamina < jumpCost)
        {
            canJump = false;
            playerController.SetCanJump(false);
            //Invokes "DelayStaminaRegen" after a delay, sets "isRegenerating" to true -> creates regen delay. 
            Invoke("DelayStaminaRegen", regenTime);
            isRegenerating = true;
        }
    }

    //Function that is called when Lshift is pressed (from the playerController)
    private void Sprinting(bool isRunning)
    {
        //Sets the bool "sprinting" from Stamina script = the bool isRunning which becomes true when Lshift has been pressed.
        sprinting = isRunning;
        if(!isRunning && !isRegenerating)
        {
            //If "DelayRegenCoroutine" is not null, then it will be stopped, else it will start.
            //"DelayRegenCoroutine" monitors the "DelayRegenerating()" coroutine. 
            if(DelayRegenCoroutine != null) { StopCoroutine(DelayRegenCoroutine); }
            DelayRegenCoroutine = StartCoroutine(DelayRegenerating());         
        }
    }

    //Coroutine that delays regeneration
    IEnumerator DelayRegenerating()
    {
        isRegenerating = false;
        yield return new WaitForSeconds(regenTime);
        isRegenerating = true;
        //Stops itself after execution
        DelayRegenCoroutine = null;
    }

    IEnumerator Stamina()
    {
        //Works like an update function, because the while loop will always be true
        while(true)
        {
            if (isRegenerating)
            {
                //Checks all the conditions for when the player should start to regenerate.
                if ((!sprinting) || (sprinting && !isMoving) || (playerStamina < maxStamina && sprinting) || canJump)
                {
                    playerController.SetCanRun(true);
                    playerStamina += staminaRegen * Time.deltaTime; //Stamina regenerates

                    //If "playerStamina" is greater than or equal to the jumpCost, they can jump
                    if(playerStamina >= jumpCost)
                    {
                        canJump = true;
                        playerController.SetCanJump(true);
                    }
                    UpdateStaminaUI(1);
                }
                
                //If the playerStamina reaches maxStamina (or above), "playerStamina = maxStamina" and UI becomes invisible.
                if (playerStamina >= maxStamina)
                {
                    playerStamina = maxStamina;
                    UpdateStaminaUI(0);
                }
            }
            else //(!isRegenerating)
            {
                if (playerStamina <= maxStamina)
                {
                    if (sprinting && isMoving)
                    { 
                        playerController.SetCanRun(true);
                        playerStamina -= staminaDrain * Time.deltaTime; //Stamina drain
                        UpdateStaminaUI(1);

                        //If playerStamina is less than jumpCost, they cannot jump.
                        if (playerStamina < jumpCost)
                        {
                            canJump = false;
                            playerController.SetCanJump(false);
                        }

                        //If playerStamina is less than 0, they cannot run/sprint
                        if (playerStamina <= 0)
                        {
                            playerStamina = 0;
                            sprinting = false;
                            playerController.SetCanRun(false);
                            
                            //This prevents from instant regeneration when playerStamina is less than 0 and sprinting is false. 
                            //Because sprinting is being set to true, it is as if the player is running -> isRegenerating is never set to true.
                            if (playerStamina <= 0 && !sprinting)
                            {    
                                sprinting = true;
                            }
                        }
                    }
                }
                
            }

            yield return null;
        }
    }
    
    //Function that is invoked to create regeneration delay. 
    public void DelayStaminaRegen()
    {
        isRegenerating = true;
    }

    private void OnEnable()
    {
        playerController.onSprint += Sprinting;
        playerController.onJump += Jumping;
    }

    private void OnDisable()
    {
        playerController.onSprint -= Sprinting;
        playerController.onJump -= Jumping;
    }

    //Updates the UI if the Stamina bar
    void UpdateStaminaUI(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
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
