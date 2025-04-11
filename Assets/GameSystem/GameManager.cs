using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject pauseGame;

    private bool isPaused;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        pauseGame.SetActive(false);
        PlayerInputHandler.instance.uiAction.Pause.performed += OnPausePerformed;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.instance.uiAction.Pause.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseGame.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
