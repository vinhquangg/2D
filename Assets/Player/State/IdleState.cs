using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MoveState
{
    public IdleState(PlayerStateMachine playerState) : base(playerState) { }

    public override void EnterState()
    {
        base.EnterState();

        if (playerState.rb.velocity.magnitude == 0)
        {
            playerState.anim.SetBool("isMove", false);
            playerState.PlayAnimation("Idle");
        }

        if (playerState.anim.GetBool("isAttack1")) return;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (playerState.rb.velocity.magnitude > 0.1f)
        {
            playerState.SwitchState(new MoveState(playerState));
        }
    }


    public override void ExitState()
    {
        base.ExitState();
        if(movementInput != Vector2.zero)
        {
            playerState.anim.SetBool("isIdle", false);
            playerState.anim.SetBool("isMove", true);
            playerState.PlayAnimation("Run");
        }
    }

    protected override void CanAttack()
    {
        base.CanAttack();
    }

    public override void HandleInput()
    {
        base.HandleInput();

    }
}
