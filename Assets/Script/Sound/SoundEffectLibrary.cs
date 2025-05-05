using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundEffectDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundEffectDictionary = new Dictionary<string, List<AudioClip>>();

        foreach(SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundEffectDictionary[soundEffectGroup.name] = soundEffectGroup.soundEffects;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundEffectDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundEffectDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }

    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> soundEffects;
    }
}
