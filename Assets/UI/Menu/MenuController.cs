using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private TextMeshProUGUI nofileSave;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject noSavePanel;

    private const string musicVolumeKey = "Menu";
    private const string effectVolumeKey = "Effect";

    [Header("Save/Load")]
    [SerializeField] private string saveFileName = "save_game.json";

    private void Start()
    {
        // Ẩn cả hai panel ban đầu
        if (loadingPanel != null) loadingPanel.SetActive(false);
        if (noSavePanel != null) noSavePanel.SetActive(false);
        //BrightnessSettings.LoadBrightness();
        BrightnessSettings.ApplyToVolume(postProcessingVolume);
    }

    private void OnEnable()
    {
        if (musicSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(musicVolumeKey, 1f);
            musicSlider.value = savedVolume;
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (effectSlider != null)
        {
            float savedEffectVolume = PlayerPrefs.GetFloat(effectVolumeKey, 1f);
            effectSlider.value = savedEffectVolume;
            effectSlider.onValueChanged.RemoveAllListeners();
            effectSlider.onValueChanged.AddListener(SetEffectVolume);
        }

        if (brightnessSlider != null)
        {
           

            brightnessSlider.minValue = -2f;
            brightnessSlider.maxValue = 2f;

            float savedBrightness = BrightnessSettings.GetBrightness();
            brightnessSlider.value = savedBrightness;

            if (postProcessingVolume == null)
            {
                postProcessingVolume = FindObjectOfType<Volume>();
                if (postProcessingVolume == null)
                {
                    Debug.LogWarning("Không tìm thấy Volume trong scene.");
                }
                else
                {
                    BrightnessSettings.ApplyToVolume(postProcessingVolume);
                }
            }
            else
            {
                BrightnessSettings.ApplyToVolume(postProcessingVolume);
            }

            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    public void SetBrightness(float value)
    {
        BrightnessSettings.SetBrightness(postProcessingVolume, value);
    }


    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1f);
        float db = Mathf.Log10(volume) * 20f;
        AudioManager.Instance.audioMixer.SetFloat("Menu", db);

        PlayerPrefs.SetFloat(musicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetEffectVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1f);
        float db = Mathf.Log10(volume) * 20f;
        AudioManager.Instance.audioMixer.SetFloat("Effect", db);

        PlayerPrefs.SetFloat(effectVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void NewGame()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted old save file: " + path);
        }

        PlayerSaveTemp.tempData = null;

        SceneManager.sceneLoaded += OnSceneLoadedAfterNewGame;
        SceneLoader.instance.LoadScene(SceneName.SampleScene);
    }

    private void OnSceneLoadedAfterNewGame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedAfterNewGame;

        var player = PlayerManager.Instance?.GetCurrentPlayer();
        if (player != null)
        {
            player.GetComponent<PlayerSoul>()?.SetSoul(0);
        }
    }


    public void LoadGame()
    {
        string path = GetSavePath();



        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found.");

            if (noSavePanel != null)
            {
                noSavePanel.SetActive(true); 
            }

            return;
        }


        loadingPanel.SetActive(true);
        

        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        SceneManager.sceneLoaded += OnSceneLoadedAfterLoadGame;
        SceneManager.LoadScene(saveData.player.currentSceneName);
        Time.timeScale = 1;
        GameManager.instance.ShowPlayerUI();
    }


    private void OnSceneLoadedAfterLoadGame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedAfterLoadGame;

        if (SaveLoadManager.instance != null)
        {
            SaveLoadManager.instance.LoadAfterSceneLoaded();
        }
        else
        {
            Debug.LogError("SaveLoadManager is null in loaded scene");
        }
    }

    public void SaveGame()
    {
        SaveLoadManager.instance.SaveGame();
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        GameManager.instance.HidePlayerUI();
        SceneLoader.instance.LoadScene(SceneName.Menu);
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
