using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions
{
    public static Transform FindFirstParent(this Transform parent)
    {
        Transform t = parent;

        while (t.parent != null)
        {
            t = t.parent;
        }

        return t;
    }

    public static List<T> GetChildrenRecursive<T>(this Transform _this, bool returnParent = true) where T : Component
    {
        List<T> returns = new List<T>();
        if (returnParent == true)
        {
            T component = _this.GetComponent<T>();
            if(component != null) { returns.Add(component); }
        }

        foreach (Transform t in _this)
        {
            returns.AddRange(t.GetChildrenRecursive<T>(true));
        }

        return returns;
    }

    public static void PlayRandomClip(this AudioSource source, AudioClip[] clips)
    {
        source.pitch = 1f;
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
    public static void PlayRandomClip(this AudioSource source, AudioClip[] clips, bool randomPitch)
    {
        source.pitch = randomPitch ? Random.Range(0.85f, 1.25f) : 1f;
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public static void PlayClipWithRandomPitch(this AudioSource source, AudioClip clip)
    {
        source.pitch = Random.Range(0.85f, 1.25f);
        source.PlayOneShot(clip);
    }
}