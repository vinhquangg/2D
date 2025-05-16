using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput InputActions { get; private set; }
    public PlayerInput.PlayerActions playerAction { get; private set; }
    public PlayerInput.UIActions uiAction { get; private set; }

    public static PlayerInputHandler instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
        InputActions = new PlayerInput();
        playerAction = InputActions.Player;
        uiAction = InputActions.UI;
    }
    
    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    public void DisablePlayerInput()
    {
        playerAction.Disable();
    }

    public void EnablePlayerInput()
    {
        playerAction.Enable();
    }
}
