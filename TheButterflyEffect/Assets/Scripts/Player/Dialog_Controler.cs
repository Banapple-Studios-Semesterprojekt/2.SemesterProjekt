using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Controler : MonoBehaviour
{
    [SerializeField] private Dialogue testdialogue;
    private Dialogue_Triggers DiaTrigger;
    [SerializeField] private TextMeshProUGUI DisplayText;
    [SerializeField] private float scentensSwitchSpeed;
    private float letterSpeed=0.02f;
    public bool activeDialogue;
    private bool skip;
    [SerializeField] private Image Backkground;
    private void Start()
    {
        DisplayText.text = "";
        DiaTrigger= GameObject.Find("DialogueManager").GetComponent<Dialogue_Triggers>();
       
    }
    public void RunDialogue(Dialogue dialogue, Color color,bool isMoreDialogue)
    {
        DisplayText.color = color;
        StartCoroutine(DisplayDialgue(dialogue.sentences,isMoreDialogue));
        Backkground.enabled = true;
    }
    IEnumerator DisplayDialgue(string[] dia,bool isMoreDialogue)
    {
        activeDialogue = true;
        for (int i = 0; i < dia.Length; i++)
        {
            string DialogueText="";
            skip = false;
            char[] CharText = dia[i].ToCharArray();
            for (int j = 0; j < CharText.Length; j++)
            {
                DialogueText=DialogueText + CharText[j];
                DisplayText.text = DialogueText;
                if (!skip)
                {
                   yield return new WaitForSeconds(letterSpeed);
                }   
            }
            yield return new WaitForSeconds(scentensSwitchSpeed);
        }
        DisplayText.text = "";
        activeDialogue =false;
        if (isMoreDialogue)
        {
            DiaTrigger.NextDialogue();
        }
        else
        {
            Backkground.enabled = false;
        }
    }
    private void Update()
    {
        if (activeDialogue&&Input.GetKeyDown(KeyCode.F))
        {
            skip = true;    
        }
        

    }
}
