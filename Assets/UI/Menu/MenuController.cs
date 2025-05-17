using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    //[SerializeField] private Slider sfxSlider;
    //[SerializeField] private Slider sfxSlider;
    private string saveFileName = "save_game.json";
    private void Start()
    {
        if(soundSlider != null)
        {
            SoundManager.SetSFXSlider(soundSlider);
        }
        
    }
    public void LoadGame()
    {
        string path = GetSavePath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // Load đúng scene lưu trong SaveData
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

    public void NewGame()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted old save file: " + path);
        }


        PlayerSaveTemp.tempData = null;
        SceneLoader.instance.LoadScene(SceneName.SampleScene);
        //SaveLoadManager.instance.NewGame();


        //GameManager.instance.ShowPlayerUI();
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
