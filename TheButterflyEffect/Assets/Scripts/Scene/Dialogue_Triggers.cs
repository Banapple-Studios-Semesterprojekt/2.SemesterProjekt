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
        if (dialoguePage < dialogues[activeDialogueGroup].dialouge.Count-1)
        {
            Return = true;
        }
        contoler.RunDialogue(dialogues[activeDialogueGroup].dialouge[dialoguePage].dialouge, dialogues[activeDialogueGroup].dialouge[dialoguePage].color, Return);
        Debug.Log(Return);
        dialoguePage++;
        if (!Return)
        {
            dialoguePage = 0;
        }
     }


}

