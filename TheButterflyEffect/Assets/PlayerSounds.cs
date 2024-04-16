using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource feetAudio;
    [SerializeField] private AudioSource heldItemAudio;

    [Header("Player Sounds Properties")]
    [SerializeField] [Range(0.1f, 2f)] private float walkDelay = 0.5f;
    [SerializeField][Range(0.1f, 1f)] private float runDelay = 0.25f;
    [SerializeField][Range(0.5f, 3f)] private float crouchDelay = 1f;

    private float pc_currentSpeed, pc_walkSpeed, pc_runSpeed, pc_crouchSpeed;


    //Audio Clips
    private AudioClip[] pineFootsteps;
    private AudioClip swingClip;
    private AudioClip flashlightClick;

    //Variables to determine when to play sounds
    private CharacterController cc;
    private PlayerController pc;
    private float moveTime = 0f;
    private float initVol;

    private string itemInHand;

    private void Start()
    {
        //References
        pineFootsteps = AudioManager.Instance().data.pineFootsteps;
        swingClip = AudioManager.Instance().data.swingSFX;
        flashlightClick = AudioManager.Instance().data.flashlightClick;
        pc = PlayerController.Instance();
        cc = pc.GetComponent<CharacterController>();

        //Variables Setup
        pc_currentSpeed = pc.GetCurrentSpeed();
        pc_walkSpeed = pc.GetWalkSpeed();
        pc_runSpeed = pc.GetRunSpeed();
        pc_crouchSpeed = pc.GetCrouchSpeed();
        moveTime = 0f;
        initVol = feetAudio.volume;

        //Events
        HeldItem heldItem = GetComponent<HeldItem>();
        heldItem.onHoldItem += HeldItem_onHoldItem;
        heldItem.onPrimaryAction += HeldItem_onPrimaryAction;
    }

    private void HeldItem_onHoldItem(string type)
    {
        itemInHand = type;
    }

    private void HeldItem_onPrimaryAction()
    {
        if(itemInHand == "Glowstick")
        {
            heldItemAudio.PlayClipWithRandomPitch(flashlightClick);
        }
    }

    private void Update()
    {
        UpdateFootstepSounds();
    }

    private void UpdateFootstepSounds()
    {
        pc_currentSpeed = pc.GetCurrentSpeed();

        if(cc.velocity.sqrMagnitude > 0.1f && cc.isGrounded)
        {
            moveTime += Time.deltaTime;
            if(moveTime > GetCurrentDelay())
            {
                PlayFootsteps();
                moveTime = 0f;
            }
        }
    }

    public void PlayFootsteps()
    {
        feetAudio.volume = GetAppropiateVolume();
        feetAudio.PlayRandomClip(pineFootsteps, true);
    }

    private float GetCurrentDelay()
    {
        return pc_currentSpeed == pc_walkSpeed ? walkDelay : pc_currentSpeed == pc_runSpeed ? runDelay : crouchDelay;
    }
    private float GetAppropiateVolume()
    {
        return GetCurrentDelay() == walkDelay ? initVol * 0.7f : GetCurrentDelay() == runDelay ? initVol * 1f : initVol * 0.3f;
    }
}