using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixerGroup[] mixerGroups;

    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;

            // Set the appropriate mixer group for the sound
            if (s.isMusic && mixerGroups[0] != null)
            {
                s.source.outputAudioMixerGroup = mixerGroups[0];
            }
            else if (s.isSFX && mixerGroups[1] != null)
            {
                s.source.outputAudioMixerGroup = mixerGroups[1];
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }

    }

    public void Mute(bool mute)
    {
        float volume = mute ? 0f : 1f;
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * volume;
        }
    }

}
