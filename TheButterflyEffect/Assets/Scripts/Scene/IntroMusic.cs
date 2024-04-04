using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusic : MonoBehaviour
{
    private AudioSource music;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        StartCoroutine(CheckIfShouldDestroy());
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator CheckIfShouldDestroy()
    {
        while(music.isPlaying)
        {
            yield return new WaitForSeconds(3);
        }

        Destroy(gameObject);
    }
}
