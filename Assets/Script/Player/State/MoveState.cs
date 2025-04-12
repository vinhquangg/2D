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
        if (playerState.anim.GetBool("Attack")) return;
        Debug.Log("State: " + GetType().Name); 
    }

    public virtual void UpdateState()
    {
        HandleInput();

        if (playerState.rb.velocity.magnitude < 0.1f && !(playerState.currentState is IdleState))
        {
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
        CanAttack();
    }
    public virtual void ExitState()
    {
        playerState.anim.SetBool("isMove", false);
        //playerState.PlayAnimation("Idle");
    }

    private void ReadMoveInput()
    {
        movementInput = PlayerInputHandler.instance.playerAction.Move.ReadValue<Vector2>();
    }
    public void Move()
    {
        playerState.rb.velocity = (movementInput.normalized*playerState.playerData.moveSpeed);
    }

    protected virtual void CanAttack()
    {
        playerState.TryAttack();
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


}
