using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}