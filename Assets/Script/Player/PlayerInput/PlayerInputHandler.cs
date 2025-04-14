using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput InputActions { get; private set; }
    public PlayerInput.PlayerActions playerAction { get; private set; }

    public static PlayerInputHandler instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        InputActions = new PlayerInput();
        playerAction = InputActions.Player;
    }
    
    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}
