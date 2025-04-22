using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject playerInputPrefab;
    public GameObject pauseGamePrefab;
    public GameObject playerUI;

    private GameObject pauseGameInstance;
    private bool isPaused;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo PlayerInput nếu chưa có
        if (FindObjectOfType<PlayerInputHandler>() == null)
        {
            var input = Instantiate(playerInputPrefab);
            DontDestroyOnLoad(input);
        }

        // Khởi tạo PauseMenu nếu chưa có
        if (FindObjectOfType<MenuController>() == null)
        {
            pauseGameInstance = Instantiate(pauseGamePrefab);
            DontDestroyOnLoad(pauseGameInstance);
        }

        // Khởi tạo PlayerUI nếu chưa có
        if (GameObject.FindGameObjectWithTag("PlayerUI") == null)
        {
            var playerUIInstance = Instantiate(playerUI);
            DontDestroyOnLoad(playerUIInstance);
        }
        else
        {
            pauseGameInstance = FindObjectOfType<MenuController>().gameObject;
        }
    }

    private void Start()
    {
        if (pauseGameInstance != null)
        {
            pauseGameInstance.SetActive(false);
        }

        PlayerInputHandler.instance.uiAction.Pause.performed += OnPausePerformed;
    }

    private void OnDestroy()
    {
        if (PlayerInputHandler.instance != null)
            PlayerInputHandler.instance.uiAction.Pause.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseGameInstance != null)
            pauseGameInstance.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;
    }
}
