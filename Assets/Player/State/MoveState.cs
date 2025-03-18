using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MoveState : IPlayerState
{
    protected PlayerStateMachine playerState;
    protected Vector2 movementInput;

    public MoveState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
    }
    public virtual void EnterState()
    {
        playerState.anim.enabled = true;
        Debug.Log("State: " + GetType().Name); 
    }

    public virtual void UpdateState()
    {
        HandleInput();
        if (movementInput == Vector2.zero)
        {
            if (playerState.currentState is IdleState)
            {
                return;
            }
            playerState.SwitchState(new IdleState(playerState));

        }
    }

    public virtual void HandleInput()
    {
        ReadMoveInput();
    }

    public virtual void PhysicsUpdate()
    {
        Flip();
        Move();
    }
    public virtual void ExitState()
    {
        playerState.anim.SetBool("isMove", false);
        playerState.anim.Play("Idle");
    }

    private void ReadMoveInput()
    {
        movementInput = PlayerInputHandler.instance.playerAction.Move.ReadValue<Vector2>();
    }
    private void Move()
    {
        playerState.rb.velocity = (movementInput*playerState.playerData.moveSpeed).normalized;
    }

    private void Flip()
    {
        if ((movementInput.x > 0 && playerState.transform.localScale.x < 0) ||
            (movementInput.x < 0 && playerState.transform.localScale.x > 0))
        {
            Vector3 localScale = playerState.transform.localScale;
            localScale.x *= -1;
            playerState.transform.localScale = localScale;
        }
    }


    protected void PlayAnimation(string animName)
    {
        if (playerState.anim == null) return;
     
        AnimatorStateInfo stateInfo = playerState.anim.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.fullPathHash != 0 && !stateInfo.IsName(animName))
        {
            playerState.anim.Play(animName);
        }
    }

}
