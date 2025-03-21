using System.Collections;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine playerState;
    private int attackIndex = 0;
    private string[] attackAnimations = { "Attack_Spear", "Attack_Sword" };
    private float comboResetTime = 0.2f;
    private float lastAttackTime;
    private bool isAttacking = false;

    public AttackState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
    }

    public void EnterState()
    {
        playerState.anim.enabled = true;
        attackIndex = playerState.anim.GetInteger("AttackIndex");
        isAttacking = true;
        playerState.rb.velocity = Vector2.zero;
        playerState.rb.isKinematic = true;
        playerState.PlayAnimation("Attack_Spear");
        //PlayNextAttack();
        lastAttackTime = Time.time;
        playerState.StartCoroutine(AttackRoutine());
    }

    public void ExitState()
    {
        playerState.rb.isKinematic = false;
        isAttacking = false;
        playerState.anim.SetInteger("AttackIndex", 0);
    }

    public void HandleInput()
    {
        if (Time.time - lastAttackTime < comboResetTime && playerState.isAttackPressed)
        {
            lastAttackTime = Time.time;

            if (attackIndex < attackAnimations.Length - 1)
            {
                attackIndex++;
                playerState.anim.SetInteger("AttackIndex", attackIndex);
                //PlayNextAttack();
            }
        }
    }

    public void PhysicsUpdate() 
    {
    }

    public void UpdateState()
    {
        HandleInput();
    }

    public void PlayNextAttack()
    {
        string attackAnim = attackAnimations[attackIndex];

        playerState.anim.SetTrigger("Attack");
        playerState.PlayAnimation(attackAnim);
    }

    private IEnumerator AttackRoutine()
    {
        while (playerState.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            yield return null;
        }

        if (attackIndex == 1 && Time.time - lastAttackTime < comboResetTime)
        {
            yield return new WaitForSeconds(0.2f);
            attackIndex = 0;
            playerState.anim.SetInteger("AttackIndex", attackIndex);
            PlayNextAttack();
            //playerState.SwitchState(new IdleState(playerState));
        }
        else
        {
            isAttacking = false;
            attackIndex = 0;
            playerState.anim.SetInteger("AttackIndex", attackIndex);
        }
        playerState.SwitchState(new IdleState(playerState));
    }
}
