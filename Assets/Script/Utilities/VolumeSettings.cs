using UnityEngine;
using UnityEngine.Audio;

public static class VolumeSettings
{
    private const string MusicVolumeKey = "Menu";
    private const string SFXVolumeKey = "Effect";

    public static void ApplySavedVolumes(AudioMixer mixer)
    {
        SetVolume(mixer, MusicVolumeKey, "Menu");
        SetVolume(mixer, SFXVolumeKey, "Effect");
    }

    public static float GetVolume(string key)
    {
        return PlayerPrefs.GetFloat(key, 1f);
    }

    public static void SetVolume(AudioMixer mixer, string prefsKey, string exposedParam)
    {
        float volume = PlayerPrefs.GetFloat(prefsKey, 1f);
        float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(exposedParam, db);
    }

    public static void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
}
