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
    //public float delayStamina = 3;
    public float multiplier;
    public bool isRegenerating;
    public float regenTime = 3f;

    [Header("Stamina UI")]
    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;

    private Coroutine StaminaCoroutine;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCharacterController = GetComponent<CharacterController>();

        StaminaCoroutine = StartCoroutine(Stamina());
    }

    private void Update()
    {
        isMoving = playerCharacterController.velocity.sqrMagnitude > 0;
        isRegenerating = !sprinting ^ (sprinting && !isMoving);
        canJump = playerStamina >= maxStamina * jumpCost / maxStamina;

        if (isRegenerating)
        {
            if (StaminaCoroutine == null)
                StartCoroutine(Stamina());
        }
        else
        {
            if (StaminaCoroutine != null)
                StopCoroutine(Stamina());
        }
    }
   

    private void Jumping()
    {
        if(canJump && playerCharacterController.isGrounded)
        StartCoroutine(DelayedJumpAndRegen());
    }

    private void Sprinting(bool isRunning)
    {
        sprinting = isRunning;

    }

    private IEnumerator DelayedJumpAndRegen()
    {
        canJump = false;

        playerStamina -= jumpCost;
        UpdateStaminaUI(1);   
 
        yield return new WaitForSeconds(regenTime);
        
        DelayStaminaRegen();
        canJump = true;
    }

    

    IEnumerator Stamina()
    {
        float timeSinceLastRegeneration = 0f;
        bool delayTime = true;

        while(playerStamina <= maxStamina)
        {

            timeSinceLastRegeneration += Time.deltaTime;

            if (timeSinceLastRegeneration >= regenTime)
                delayTime = true;
            else
                delayTime = false;

            if (isRegenerating)
            {
                if (delayTime && ((!sprinting && delayTime) || (sprinting && !isMoving) || (playerStamina < maxStamina && sprinting)))
                {
                    playerController.SetCanRun(true);
                    playerStamina += staminaRegen * Time.deltaTime;
                    UpdateStaminaUI(1);

                }

                if (playerStamina >= maxStamina)
                {
                    playerStamina = maxStamina;
                    UpdateStaminaUI(0);
                }
            }
            else 
            {
                timeSinceLastRegeneration = 0f;

                if (playerStamina <= maxStamina)
                {
                    if (sprinting && isMoving)
                    { 
                        playerController.SetCanRun(true);
                        playerStamina -= staminaDrain * Time.deltaTime;
                        UpdateStaminaUI(1);
                        if (playerStamina <= 0)
                        {
                            playerStamina = 0;
                            sprinting = false;
                            playerController.SetCanRun(false);
                            
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
