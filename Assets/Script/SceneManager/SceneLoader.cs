using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
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
        StartCoroutine(LoadSceneFromSaveRoutine(currentSceneName));
    }

    private IEnumerator LoadSceneRoutine(SceneName sceneName)
    {
        if (sceneName == SceneName.Menu)
        {
            PlayerInputHandler.instance?.DisablePlayerInput();
            GameManager.instance.TogglePause();
            yield return SceneManager.LoadSceneAsync(sceneName.ToString());
            yield return null;

            if (AudioManager.Instance?.background != null)
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.background);
                VolumeSettings.ApplySavedVolumes(AudioManager.Instance.audioMixer);
            }
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneName.ToString());
            PlayerInputHandler.instance?.EnablePlayerInput();
            yield return null;

            BrightnessSettings.ApplyToVolume(FindObjectOfType<Volume>());
            if (AudioManager.Instance?.play != null)
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.play);
                VolumeSettings.ApplySavedVolumes(AudioManager.Instance.audioMixer);
            }

            Vector3 spawnPos = (PlayerSaveTemp.tempData != null) ? PlayerSaveTemp.tempData.position : Vector3.zero;
            PlayerManager.Instance.SpawnPlayer(spawnPos);

            yield return null;

            if (!SaveLoadManager.IsLoading)
            {
                var playerObj = PlayerManager.Instance.GetCurrentPlayer();
                var stateMachine = playerObj?.GetComponent<PlayerStateMachine>();

                if (PlayerSaveTemp.tempData != null)
                {
                    stateMachine?.LoadFromData(PlayerSaveTemp.tempData);
                }
                else if (stateMachine != null)
                {
                    var defaultData = PlayerManager.Instance.GetDefaultPlayer();
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

    private IEnumerator LoadSceneFromSaveRoutine(SceneName sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName.ToString());
        yield return null;

        BrightnessSettings.ApplyToVolume(FindObjectOfType<Volume>());
        //GameManager.instance?.TogglePause();

        SaveLoadManager.instance?.LoadAfterSceneLoaded();
    }

    //public SceneName GetCurrentScene()
    //{
    //    return currentSceneName;
    //}
}
