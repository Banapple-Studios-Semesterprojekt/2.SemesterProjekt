using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialgoue;
    [SerializeField] private float delayBeforeTrigger = 3f;

    private void Start()
    {
        Invoke(nameof(StartDialogue), delayBeforeTrigger);
    }

    private void StartDialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialgoue);
    }
}
