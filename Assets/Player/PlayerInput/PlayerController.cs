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

    // Update is called once per frame
    void Update()
    {
        playerState.currentState.UpdateState();
    }
    void FixedUpdate()
    {
        playerState.currentState.PhysicsUpdate();
    }
}
