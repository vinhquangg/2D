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
        if (sceneName == SceneName.Menu)
        {
            GameManager.instance.TogglePause();
            SoundManager.Play("Menu");
        }

        yield return SceneManager.LoadSceneAsync(sceneName.ToString());
        yield return null;

        if (sceneName != SceneName.Menu)
        {
            SoundManager.Play("Play");
            Vector3 spawnPos = (PlayerSaveTemp.tempData != null) ? PlayerSaveTemp.tempData.position : Vector3.zero;


            PlayerManager.Instance.SpawnPlayer(spawnPos);

            yield return null;

            if (PlayerSaveTemp.tempData == null)
            {
                var playerObj = PlayerManager.Instance.GetCurrentPlayer();
                var stateMachine = playerObj?.GetComponent<PlayerStateMachine>();
                if (stateMachine != null)
                {
                    var defaultData = stateMachine.GetDefaultPlayerData();
                    if (defaultData != null)
                    {
                        PlayerSaveTemp.tempData = defaultData;
                        stateMachine.LoadFromData(defaultData);
                    }
                }
            }
            GameManager.instance?.ShowPlayerUI();
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
