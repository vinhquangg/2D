using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private static AudioSource audioSource;
    private SoundEffectLibrary soundEffectLibrary;

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

    private void OnDestroy()
    {
        if (instance == this)
        {

        }
    }

    private void OnVolumeChanged(float newVolume)
    {
        audioSource.volume = newVolume;
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = instance.soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetSFXSlider(Slider slider)
    {
        if (slider == null || instance == null) return;



        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(value =>
        {

        });
    }
}
