using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundScr
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
