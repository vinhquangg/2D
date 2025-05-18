using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class BrightnessSettings
{
    private const string BrightnessKey = "Brightness";

    private static float currentBrightness = 0f;

    public static float GetBrightness()
    {
        return currentBrightness;
    }

    public static void LoadBrightness()
    {
        currentBrightness = PlayerPrefs.GetFloat(BrightnessKey, 0f);
    }

    public static void SaveBrightness()
    {
        PlayerPrefs.SetFloat(BrightnessKey, currentBrightness);
        PlayerPrefs.Save();
    }

    public static void SetBrightness(Volume volume, float value)
    {
        currentBrightness = value;

        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = value;
            }
        }
    }

    public static void ApplyToVolume(Volume volume)
    {
        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = currentBrightness;
            }
            else
            {
                Debug.LogWarning("Volume không có ColorAdjustments.");
            }
        }
    }
}
