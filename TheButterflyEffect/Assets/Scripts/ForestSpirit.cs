using System.Collections;
using UnityEngine;

public class ForestSpirit : MonoBehaviour
{
    private Animator animator;
    private float waitTime = 5f;
    private Vector3 forestSpiritPosition;
    private Transform playerCamera;
    private float smoothTime;
    private Canvas playerCanvas;
    private GameObject inventoryUI;
    private GameObject dialogueUI;
    private Dialogue_Triggers dialogueTriggers;
    private Dialog_Controler dialogueController;
    private Renderer forestSpiritRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerCamera = PlayerController.Instance().GetCamera();
        playerCanvas = PlayerController.Instance().GetComponentInChildren<Canvas>();
        dialogueTriggers = FindAnyObjectByType<Dialogue_Triggers>();
        dialogueController = FindAnyObjectByType<Dialog_Controler>();
        inventoryUI = playerCanvas.transform.Find("Inventory UI").gameObject;
        dialogueUI = playerCanvas.transform.Find("Dialogue UI").gameObject;
        forestSpiritRenderer = GetComponentInChildren<Renderer>();

        forestSpiritRenderer.enabled = false;

        StartCoroutine(Spirit());
    }
    IEnumerator Spirit()
    {
        yield return new WaitForSeconds(waitTime);

        //Sets spawn position of Forest Spirit close to the player
        Vector3 unitSphere = Random.insideUnitSphere * 3;
        unitSphere.y = 0;
        transform.position = unitSphere + playerCamera.transform.position + Vector3.up * 5;
        forestSpiritPosition = transform.position;
        forestSpiritRenderer.enabled = true;

        while(Vector3.Distance(transform.position, playerCamera.position + playerCamera.forward * 2) > 0.2f)
        {
            Vector3 playerCamPosition = playerCamera.position + playerCamera.forward * 2;
            smoothTime += Time.deltaTime;
            transform.position = Vector3.Lerp(forestSpiritPosition, playerCamPosition, smoothTime);
            transform.LookAt(playerCamera);

            yield return null;
        }
        animator.SetBool("isIdle", true);

        //cutscene, when done it should fly away
        PlayerController.Instance().enabled = false;
        inventoryUI.SetActive(false);
        dialogueUI.SetActive(true);
        dialogueTriggers.runDialogue(0); //Starts dialogue

        while (dialogueController.activeDialogue)
        {
            //Will loop this until activeDialogue becomes false. 
            yield return new WaitForSeconds(0.1f);
        }

        //Spirit flies away
        PlayerController.Instance().enabled = true;
        inventoryUI.SetActive(true);
        dialogueUI.SetActive(false);
        animator.SetBool("isIdle", false);
        float timer = 0;
        while(timer < 30f)
        {
            timer += Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime * waitTime, Space.World);
            yield return null;
        }
        gameObject.SetActive(false);
        
    }
}
