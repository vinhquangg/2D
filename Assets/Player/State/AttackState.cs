using System.Collections;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine playerState;

    // 🔹 Biến kiểm soát Combo Attack
    private static int noOfClick = 0; // Số lần bấm Attack để thực hiện combo
    private float lastClickedTime = 0f; // Thời điểm bấm Attack gần nhất
    private float comboResetTime = 0.7f; // Nếu không bấm trong thời gian này, reset combo

    private bool canAttack = true; // Kiểm tra có thể Attack tiếp không

    public AttackState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
    }

    public void EnterState()
    {
        playerState.anim.enabled = true;
        canAttack = false;
        PerformAttack();
    }

    public void HandleInput()
    {

        if (canAttack && PlayerInputHandler.instance.playerAction.Attack.WasPressedThisFrame())
        {
            PerformAttack();
        }
    }

    public void PhysicsUpdate()
    {

        playerState.rb.velocity = PlayerInputHandler.instance.playerAction.Move.ReadValue<Vector2>() * playerState.playerData.moveSpeed;
    }

    public void UpdateState()
    {
        AnimatorStateInfo stateInfo = playerState.anim.GetCurrentAnimatorStateInfo(0);

        
        if (noOfClick == 3 && stateInfo.normalizedTime >= 1f && stateInfo.IsName("Attack_2"))
        {
            playerState.SwitchState(new MoveState(playerState));
        }


        if (Time.time - lastClickedTime > comboResetTime)
        {
            noOfClick = 0;
        }

        
        if (noOfClick >= 2 && stateInfo.normalizedTime > 0.7f)
        {
            if (stateInfo.IsName("Attack_1"))
            {
                playerState.anim.SetBool("isAttack1", false);
                playerState.anim.SetBool("isAttack2", true);
            }
            //else if (stateInfo.IsName("Attack_2"))
            //{
            //    playerState.anim.SetBool("isAttack2", false);
            //    playerState.anim.SetBool("isAttack3", true);
            //}
        }
    }

    private void PerformAttack()
    {
        lastClickedTime = Time.time;

        if (noOfClick == 0 || Time.time - lastClickedTime > comboResetTime)
        {
            noOfClick = 1;
        }
        else
        {
            noOfClick = Mathf.Clamp(noOfClick + 1, 1, 3);
        }

        playerState.anim.SetBool("isAttack" + noOfClick, true);
        playerState.PlayAnimation("Attack_" + noOfClick);

        canAttack = false;
        playerState.StartCoroutine(EnableAttackAfterDelay(0.2f));
    }

    private IEnumerator EnableAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    public void ExitState()
    {
        noOfClick = 0; 
        canAttack = true;
    }
}
