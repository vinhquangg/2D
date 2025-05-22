using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixer audioMixer;
    [Header("Audio Source")]
    public AudioSource Background;
    public AudioSource EffectSource;
    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip shockWave;
    public AudioClip thunder;
    public AudioClip dialogue;
    [Header("Player")]
    public AudioClip hit; 
    [Header("Audio Source")]
    public AudioClip death;
    [Header("Audio Source")]
    public AudioClip play;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        VolumeSettings.ApplySavedVolumes(audioMixer);
        Background.clip = background;
        Background.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            EffectSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip != null && Background.clip != clip)
        {
            Background.clip = clip;
            Background.loop = loop;
            Background.Play();
        }
    }

    public void StopMusic()
    {
        Background.Stop();
    }

}
