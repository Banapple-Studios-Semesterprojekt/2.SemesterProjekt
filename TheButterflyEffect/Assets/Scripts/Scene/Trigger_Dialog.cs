using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : MonoBehaviour
{
    [SerializeField] private int Dialogue_Id;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            GameObject.Find("DialogueManager").GetComponent<Dialogue_Triggers>().runDialogue(Dialogue_Id);
            Debug.Log("Trigger Dialouge With Id:" + Dialogue_Id);
            Destroy(gameObject);
        }
    }
}
