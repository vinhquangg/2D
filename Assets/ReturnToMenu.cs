using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    public GameObject ReturnToMenuPrefab;
    public void ReturnToMenuWithSave()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.HidePlayerUI();
            GameManager.instance.TogglePause();
        }
        PlayerInputHandler.instance?.DisablePlayerInput();
        SceneLoader.instance.LoadScene(SceneName.Menu);
    }

    public void ShowWinPanelDelayedExternally(GameObject ReturnToMenuPrefab)
    {
        StartCoroutine(ShowWinPanelRoutine(ReturnToMenuPrefab));
    }

    private IEnumerator ShowWinPanelRoutine(GameObject ReturnToMenuPrefab)
    {
        Time.timeScale = 1f; 
        yield return new WaitForSecondsRealtime(2f);

        if (ReturnToMenuPrefab != null)
        {
            ReturnToMenuPrefab.SetActive(true);
      
        }
    }

}
