using UnityEngine;
[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    private PlayerStateMachine playerState;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); 
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        //playerState = GetComponent<PlayerStateMachine>();
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
