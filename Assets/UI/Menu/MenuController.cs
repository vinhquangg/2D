using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("saveData_save"))
        {
            return;
        }

        string json = PlayerPrefs.GetString("saveData_save");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        PlayerPrefs.SetString("pending_save_data", json);
        PlayerPrefs.Save();
       
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

    private IEnumerator WaitAndLoadDataAfterSceneLoad()
    {
        yield return null; 

        if (SaveLoadManager.instance != null)
        {
            SaveLoadManager.instance.LoadGameFromPendingData();
        }
        else
        {
            Debug.LogError("SaveLoadManager instance is null!");
        }
    }

    public void SaveGame()
    {
        SaveLoadManager.instance.SaveGame();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }

    public void NewGame()
    {
        if (PlayerPrefs.HasKey("saveData_save"))
        {
            PlayerPrefs.DeleteKey("saveData_save");
        }

        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
}
