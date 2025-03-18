using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MoveState
{
    public IdleState(PlayerStateMachine playerState) : base(playerState) { }

    public override void EnterState()
    {
        base.EnterState();
        if (playerState.rb.velocity == Vector2.zero)
        {
            playerState.anim.SetBool("isMove", false);
            PlayAnimation("Idle");
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (movementInput != Vector2.zero)
        {
            playerState.SwitchState(new MoveState(playerState));
            playerState.anim.SetBool("isMove", true);
            PlayAnimation("Run");
        }
    }

    public override void HandleInput()
    {
        base.HandleInput();

    }
}
