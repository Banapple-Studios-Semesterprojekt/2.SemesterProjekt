using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class DialogueAndColor
{
    public Dialogue dialouge;
    public Color color = Color.white;
}

[System.Serializable]
public class DialogueGroup
{
    public List<DialogueAndColor> dialouge;
}
public class Dialogue_Triggers : MonoBehaviour
{

    public List<DialogueGroup> dialogues;
    public List<DialogueGroup> dialoguesEnglish;
    public static bool danishDialog=true;
    [SerializeField] private Dialog_Controler contoler;
    private int activeDialogueGroup;
    [SerializeField] private int dialoguePage = 0;

   

void Update()
{
    if (Input.GetKeyDown(KeyCode.T))
    {
        runDialogue(0);
    }

}
    public void runDialogue(int id)
    {
        activeDialogueGroup = id;
        NextDialogue();
    }
     public void NextDialogue()
     {
        bool Return=false;
        if (danishDialog)
        {
            if (dialoguePage < dialogues[activeDialogueGroup].dialouge.Count-1)
            {
                Return = true;
            }
            contoler.RunDialogue(dialogues[activeDialogueGroup].dialouge[dialoguePage].dialouge, dialogues[activeDialogueGroup].dialouge[dialoguePage].color, Return);
        }
        else
        {
            if (dialoguePage < dialoguesEnglish[activeDialogueGroup].dialouge.Count - 1)
            {
                Return = true;
            }
            contoler.RunDialogue(dialoguesEnglish[activeDialogueGroup].dialouge[dialoguePage].dialouge, dialoguesEnglish[activeDialogueGroup].dialouge[dialoguePage].color, Return);
        }
        Debug.Log(Return);
        dialoguePage++;
        if (!Return)
        {
            dialoguePage = 0;
        }
     }

    public void SetLanguage(int i)
    {
        if (i == 0){ 
        danishDialog = true;
        }
        else
        {
            danishDialog = false;
        }
    }

}

