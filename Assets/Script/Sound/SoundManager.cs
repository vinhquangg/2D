using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private static AudioSource audioSource;
    private SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = instance.soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void Stop()
    {
        audioSource.Stop(); 
    }

    //private void Start()
    //{
    //    sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    //}

    //public static void SetVolume(float volume)
    //{
    //    audioSource.volume = volume;
    //}

    //public void OnValueChanged()
    //{
    //    SetVolume(sfxSlider.value);
    //}
}
