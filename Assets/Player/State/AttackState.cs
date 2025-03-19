using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine playerState;

    private static int noOfClick = 0;
    private float lastClickedTime = 0f;
    private float lastAttack = 0f;

    public AttackState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
    }
    public  void EnterState()
    {
        playerState.anim.enabled = true;
    }

    public void HandleInput()
    {
        
    }

    public void PhysicsUpdate()
    {
        if (noOfClick >= 2 && playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && playerState.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            playerState.anim.SetBool("isAttack1", false);
        }
        if (noOfClick >= 2 && playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && playerState.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_2"))
        {
            playerState.anim.SetBool("isAttack2", false);
        }
        if (noOfClick >= 3 && playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && playerState.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_3"))
        {
            playerState.anim.SetBool("isAttack3", false);
        }
        if (Time.time - lastClickedTime > playerState.playerData.comboResetTime)
        {
            noOfClick = 0;
        }
        if (Time.time > lastAttack)
        {
            if (PlayerInputHandler.instance.playerAction.Attack.WasPressedThisFrame())
            {
                Attack();
            }
        }

    }

    public void UpdateState()
    {

    }

    private void Attack()
    {
        lastClickedTime = Time.time;
        noOfClick++;
        if(noOfClick== 1)
        {
            playerState.anim.SetBool("isAttack1", true);
            playerState.PlayAnimation("Attack_1");
        }
        noOfClick = Mathf.Clamp(noOfClick, 0, 3);

        if(noOfClick >=2 && playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >0.7f && playerState.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            playerState.anim.SetBool("isAttack1", false);
            playerState.anim.SetBool("isAttack2", true);
        }

        if (noOfClick >= 2 && playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && playerState.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_2"))
        {
            playerState.anim.SetBool("isAttack2", false);
            playerState.anim.SetBool("isAttack3", true);
        }
    }

    public void ExitState()
    {
        noOfClick = 0;
        
    }

    

}
