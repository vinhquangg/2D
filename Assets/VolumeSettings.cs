using UnityEngine;

public static class VolumeSettings
{
    private const string VolumeKey = "SoundVolume";

    public static float Volume
    {
        get => PlayerPrefs.GetFloat(VolumeKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
            OnVolumeChanged?.Invoke(value);
        }
    }

    public static event System.Action<float> OnVolumeChanged;
}
