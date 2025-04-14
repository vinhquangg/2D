using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private SaveData saveData;
    public void LoadGame()
    {
        if(saveData == null)
        {
            Debug.Log("No save data found");
        }
    }

    public void ChangeScence()
    {
<<<<<<< HEAD
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
        if (PlayerPrefs.HasKey("saveData_save"))
        {
            PlayerPrefs.DeleteKey("saveData_save");
        }

        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
=======
       SceneManager.LoadScene("SampleScene");
>>>>>>> parent of d29f92e (SaveGame)
    }
}
