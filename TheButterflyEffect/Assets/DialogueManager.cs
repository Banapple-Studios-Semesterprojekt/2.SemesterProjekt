using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    [SerializeField] private float nextSentenceDelay = 5f;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator blackScreenAnimator;

    private void Awake()
    {
        sentences = new Queue<string>();
        dialogueText.CrossFadeAlpha(0f, 0f, true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences) 
        { 
            sentences.Enqueue(sentence); 
        }
        StartCoroutine(DisplayNextSentence());
    }

    IEnumerator DisplayNextSentence()
    {
        if(sentences.Count <= 0) 
        {
            EndDialogue();
            yield break; 
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        yield return StartCoroutine(FadeText(true));

        yield return new WaitForSeconds(nextSentenceDelay);

        yield return StartCoroutine(FadeText(false));

        yield return new WaitForSeconds(nextSentenceDelay / 2f);

        StartCoroutine(DisplayNextSentence());
    }

    IEnumerator FadeText(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        dialogueText.CrossFadeAlpha(targetAlpha, 2f, true);
        yield return new WaitForSeconds(2f);
    }

    private void EndDialogue()
    {
        blackScreenAnimator.SetTrigger("Fade");
        Debug.Log("Dialogue Finished!");
    }
}
