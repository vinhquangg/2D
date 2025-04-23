using UnityEngine;
[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private PlayerStateMachine playerState;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        playerState = GetComponent<PlayerStateMachine>();
    }
    void Start()
    {
        playerState.SwitchState(new IdleState(playerState));
    }
    void Update()
    {
        playerState.currentState.UpdateState();
    }
}
