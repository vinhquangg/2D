using UnityEngine;
[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine playerState;

    private void Awake()
    {
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
