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
       SceneManager.LoadScene("SampleScene");
    }
}
