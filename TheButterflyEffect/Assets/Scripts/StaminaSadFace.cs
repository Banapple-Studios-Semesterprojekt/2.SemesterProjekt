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
    private Coroutine DelayRegenCoroutine;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCharacterController = GetComponent<CharacterController>();

        StaminaCoroutine = StartCoroutine(Stamina());
        canJump = true;
    }

    private void Update()
    {
        isMoving = playerCharacterController.velocity.sqrMagnitude > 0;
        isRegenerating = (!sprinting ^ (sprinting && !isMoving)) && DelayRegenCoroutine == null;

    }
   

    private void Jumping()
    {
        playerStamina -= jumpCost;
        UpdateStaminaUI(1);

        if (DelayRegenCoroutine != null) { StopCoroutine(DelayRegenCoroutine); }
        DelayRegenCoroutine = StartCoroutine(DelayRegenerating());

        playerController.SetCanJump(true);
        canJump = true;
        if (playerStamina < jumpCost)
        {
            canJump = false;
            playerController.SetCanJump(false);
            Invoke("DelayStaminaRegen", regenTime);
            isRegenerating = true;
        }
    }

    private void Sprinting(bool isRunning)
    {
        sprinting = isRunning;
        if(!isRunning && !isRegenerating)
        {
            if(DelayRegenCoroutine != null) { StopCoroutine(DelayRegenCoroutine); }
            DelayRegenCoroutine = StartCoroutine(DelayRegenerating());
            
        }
    }

    IEnumerator DelayRegenerating()
    {
        isRegenerating = false;
        yield return new WaitForSeconds(regenTime);
        isRegenerating = true;
        DelayRegenCoroutine = null;
    }

    IEnumerator Stamina()
    {
        while(true)
        {
            if (isRegenerating)
            {
                if ((((!sprinting) || (sprinting && !isMoving) || (playerStamina < maxStamina && sprinting))) || canJump)
                {
                    playerController.SetCanRun(true);
                    playerStamina += staminaRegen * Time.deltaTime;

                    if(playerStamina >= jumpCost)
                    {
                        canJump = true;
                        playerController.SetCanJump(true);
                    }

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
                if (playerStamina <= maxStamina)
                {
                    if (sprinting && isMoving)
                    { 
                        playerController.SetCanRun(true);
                        playerStamina -= staminaDrain * Time.deltaTime;
                        UpdateStaminaUI(1);
                        if (playerStamina < jumpCost)
                        {
                            canJump = false;
                            playerController.SetCanJump(false);
                        }
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
