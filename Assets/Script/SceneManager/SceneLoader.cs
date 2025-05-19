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
        StartCoroutine(LoadSceneFromSaveRoutine(currentSceneName, playerData));
    }

    private IEnumerator LoadSceneRoutine(SceneName sceneName)
    {
        if (sceneName == SceneName.Menu)
        {
            PlayerInputHandler.instance?.DisablePlayerInput();
            GameManager.instance.TogglePause();
            yield return SceneManager.LoadSceneAsync(sceneName.ToString());
            yield return null;

            if (AudioManager.Instance != null && AudioManager.Instance.background != null)
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.background);
                VolumeSettings.ApplySavedVolumes(AudioManager.Instance.audioMixer);
            }
        }
        else
        {
            PlayerInputHandler.instance?.EnablePlayerInput();
            yield return SceneManager.LoadSceneAsync(sceneName.ToString());
            yield return null;

            Volume volume = FindObjectOfType<Volume>();
            if (volume != null)
            {
                BrightnessSettings.ApplyToVolume(volume);
            }

            if (AudioManager.Instance != null && AudioManager.Instance.play != null)
            {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.play);
                VolumeSettings.ApplySavedVolumes(AudioManager.Instance.audioMixer);
            }
            Vector3 spawnPos = (PlayerSaveTemp.tempData != null) ? PlayerSaveTemp.tempData.position : Vector3.zero;

            PlayerManager.Instance.SpawnPlayer(spawnPos);
            yield return null;

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

            GameManager.instance?.ShowPlayerUI();
        }
    }



    private IEnumerator LoadSceneFromSaveRoutine(SceneName sceneName, PlayerSaveData playerData)
    {
        yield return SceneManager.LoadSceneAsync(sceneName.ToString());
        yield return null;

        Volume volume = FindObjectOfType<Volume>();
        if (volume != null)
        {
            BrightnessSettings.ApplyToVolume(volume);
        }

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
