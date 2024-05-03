using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpiritGuide : MonoBehaviour
{
    [SerializeField] private TextMeshPro guideText;

    private void Start()
    {
        StartCoroutine(DisableSpirit());
    }

    IEnumerator DisableSpirit()
    {
        yield return new WaitForSeconds(10);
        guideText.enabled = false;
        yield return new WaitForSeconds(3);
        transform.FindFirstParent().gameObject.SetActive(false);
    }
}
