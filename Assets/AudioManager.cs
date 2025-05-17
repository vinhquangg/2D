using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SoundSource;
    [SerializeField] AudioSource EffectSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip shockWave;
    public AudioClip thunder;
    public AudioClip dialogue;
    public AudioClip hit;

    private void Start()
    {
        SoundSource.clip = background;
        SoundSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            EffectSource.PlayOneShot(clip);
        }
    }
}
