using UnityEngine;

[CreateAssetMenu(fileName = "AudioData")]
public class AudioData : ScriptableObject
{
    //Player Sounds
    public AudioClip[] pineFootsteps;
    public AudioClip swingSFX;
    public AudioClip flashlightClick;

    //UI Sounds
    public AudioClip glassKnock, popClick;

    //Enemy Sounds
    public AudioClip howl, grunt, bark;
}
