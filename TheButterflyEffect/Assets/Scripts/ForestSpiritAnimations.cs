using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpiritAnimations : MonoBehaviour
{
    private Animator animator;
    private float waitTime = 10f;
    private Vector3 forestSpiritPosition;
    private Transform playerCamera;
    private float smoothTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        forestSpiritPosition = transform.position;

        StartCoroutine(Spirit());
    }

    private void Update()
    {
        playerCamera = PlayerController.Instance().GetCamera();
    }
    
    IEnumerator Spirit()
    {
        yield return new WaitForSeconds(waitTime);

        while(Vector3.Distance(forestSpiritPosition, playerCamera.position + playerCamera.forward * 2) > 0.01f)
        {
            Vector3 playerCamPosition = playerCamera.position + playerCamera.forward * 2;

            smoothTime += Time.deltaTime;

            transform.position = Vector3.Lerp(forestSpiritPosition, playerCamPosition, smoothTime);
        }
        animator.SetBool("isIdle", true);

        //cutscene, when done it should fly away
        transform.Translate(Vector3.up);
    }
}
