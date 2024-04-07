using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlaySoundClip : MonoBehaviour
{
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClipWithRandomPitch(AudioClip clip)
    {
        source.PlayClipWithRandomPitch(clip);
    }
}
