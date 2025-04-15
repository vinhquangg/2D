using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour
{
    private string saveFileName = "save_game.json";

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
        SceneManager.LoadScene("Menu");
    }

    public void NewGame()
    {
        string path = GetSavePath();

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted old save file: " + path);
        }

        SceneManager.LoadScene("SampleScene"); // Hoặc tên scene đầu game bạn chọn
        Time.timeScale = 1;
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
