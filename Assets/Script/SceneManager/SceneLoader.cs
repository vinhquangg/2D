using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance { get; private set; }
    public static string nextSpawnID = "Default";

    private SceneName currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneName sceneName)
    {
        currentSceneName = sceneName;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void LoadSceneFromSave(PlayerSaveData playerData)
    {
        currentSceneName = (SceneName)System.Enum.Parse(typeof(SceneName), playerData.currentSceneName);
        StartCoroutine(LoadSceneFromSaveRoutine(currentSceneName, playerData));
    }

    private IEnumerator LoadSceneRoutine(SceneName sceneName)
    {
        if (sceneName != SceneName.Menu && PlayerManager.Instance.GetCurrentPlayer() != null)
        {
            var stateMachine = PlayerManager.Instance.GetCurrentPlayer().GetComponent<PlayerStateMachine>();
            PlayerSaveTemp.tempData = stateMachine.GetPlayerSaveData(); 
        }

        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");

        if (sceneName == SceneName.Menu)
        {
            Debug.Log("[SceneLoader] MainMenu detected - hiding UI and pausing game.");
            GameManager.instance.TogglePause();   
        }

        yield return SceneManager.LoadSceneAsync(sceneName.ToString());

        if (sceneName != SceneName.Menu)
        {

            Vector3 spawnPos = Vector3.zero;
            if (PlayerSaveTemp.tempData != null)
            {
                spawnPos = PlayerSaveTemp.tempData.position;
            }
            PlayerManager.Instance.SpawnPlayer(spawnPos);

            if (PlayerSaveTemp.tempData != null)
            {
                var newStateMachine = PlayerManager.Instance.GetCurrentPlayer().GetComponent<PlayerStateMachine>();
                newStateMachine.LoadFromData(PlayerSaveTemp.tempData);
            }
        }
    }



    private IEnumerator LoadSceneFromSaveRoutine(SceneName sceneName, PlayerSaveData playerData)
    {
        yield return SceneManager.LoadSceneAsync(sceneName.ToString());
        yield return null;

        GameManager.instance?.TogglePause();

        PlayerManager.Instance.SpawnPlayer(playerData.position);
        PlayerManager.Instance.LoadPlayerData(playerData);

        
        SaveLoadManager.instance?.LoadAfterSceneLoaded();
    }

    public SceneName GetCurrentScene()
    {
        return currentSceneName;
    }
}
