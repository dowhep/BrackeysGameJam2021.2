using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManagerScr : MonoBehaviour
{
    public SoundScr[] Sounds;
    public static AudioManagerScr instance;
    public static float volume;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        volume = PlayerPrefs.GetFloat("Volume", 1f);

        foreach (SoundScr s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume * volume;
            s.source.pitch = s.pitch;
        }
    }

    public static void UpdateVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Volume", newVolume);
        volume = newVolume;

        if (instance == null) return;
        foreach (SoundScr s in instance.Sounds)
        {
            s.source.volume = s.volume * volume;
        }
    }

    public static void PlaySound(string name)
    {
        SoundScr s = FindSound(name);
        if (s == null) return;
        if (!s.source.loop || !s.source.isPlaying)
        {
            s.source.Play();
        }
    }
    public static void PauseSound(string name)
    {
        SoundScr s = FindSound(name);
        if (s == null) return;
        s.source.Pause();
    }
    public static void UnpauseSound(string name)
    {
        SoundScr s = FindSound(name);
        if (s == null) return;
        s.source.UnPause();
    }
    public static void StopSound(string name)
    {
        SoundScr s = FindSound(name);
        if (s == null) return;
        s.source.Stop();
    }
    public static SoundScr FindSound(string name)
    {
        if (instance == null)
        {
            Debug.LogWarning("AudioManager not instantiated!");
            return null;
        }
        SoundScr s = Array.Find(instance.Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " cannot be found!");
        }
        return s;
    }
}
