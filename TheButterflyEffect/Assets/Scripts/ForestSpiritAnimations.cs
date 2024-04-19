using System.Collections;
using UnityEngine;

public class ForestSpiritAnimations : MonoBehaviour
{
    private Animator animator;
    private float waitTime = 5f;
    private Vector3 forestSpiritPosition;
    private Transform playerCamera;
    private float smoothTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        forestSpiritPosition = transform.position;
        playerCamera = PlayerController.Instance().GetCamera();

        StartCoroutine(Spirit());
    }
    IEnumerator Spirit()
    {
        yield return new WaitForSeconds(waitTime);

        while(Vector3.Distance(transform.position, playerCamera.position + playerCamera.forward * 2) > 0.2f)
        {
            Vector3 playerCamPosition = playerCamera.position + playerCamera.forward * 2;
            smoothTime += Time.deltaTime;

            transform.position = Vector3.Lerp(forestSpiritPosition, playerCamPosition, smoothTime);

            yield return null;
        }
        animator.SetBool("isIdle", true);

        yield return new WaitForSeconds(waitTime);

        //cutscene, when done it should fly away
        float timer = 0;
        while(timer < 10f)
        {
            timer += Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime * waitTime);
            yield return null;
        }
        gameObject.SetActive(false);
        
    }
}
