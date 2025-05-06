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
        // Nếu là Menu, tạm dừng game và ẩn UI
        if (sceneName == SceneName.Menu)
        {
            Debug.Log("[SceneLoader] MainMenu detected - hiding UI and pausing game.");
            GameManager.instance.TogglePause();
        }

        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        yield return SceneManager.LoadSceneAsync(sceneName.ToString());
        yield return null; // Đợi 1 frame để đảm bảo scene đã load xong

        // Spawn player sau khi scene đã load
        Vector3 spawnPos = Vector3.zero;

        if (sceneName != SceneName.Menu)
        {
            // Nếu có dữ liệu tạm, spawn đúng vị trí đã lưu
            if (PlayerSaveTemp.tempData != null)
            {
                spawnPos = PlayerSaveTemp.tempData.position;
            }
            else
            {
                spawnPos = PlayerManager.Instance.GetDefaultPlayer().position;
            }

            PlayerManager.Instance.SpawnPlayer(spawnPos);

            // Nếu có dữ liệu tạm thì load lại
            if (PlayerSaveTemp.tempData != null)
            {
                var stateMachine = PlayerManager.Instance.GetCurrentPlayer().GetComponent<PlayerStateMachine>();
                if (stateMachine != null)
                {
                    stateMachine.LoadFromData(PlayerSaveTemp.tempData);
                }
                else
                {
                    Debug.LogError("PlayerStateMachine is missing after spawning player.");
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
