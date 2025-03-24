using System.Collections;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine playerState;
    private int attackCount = 0; // Đếm số lần bấm để xác định loại vũ khí
    private string[] attackAnimations = { "Attack_Spear", "Attack_Sword" };
    private float comboResetTime = 0.4f;
    private float lastAttackTime;
    private bool isComboActive = false;

    public AttackState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
    }

    public void EnterState()
    {
        playerState.anim.enabled = true;
        attackCount = 0;
        isComboActive = false;
        playerState.rb.velocity = Vector2.zero;
        playerState.rb.isKinematic = true;

        PlayNextAttack();
        lastAttackTime = Time.time;
        playerState.StartCoroutine(AttackRoutine());
    }

    public void ExitState()
    {
        playerState.rb.isKinematic = false;
        isComboActive = false;
        attackCount = 0;
    }

    public void HandleInput()
    {
        if (Time.time - lastAttackTime < comboResetTime && playerState.isAttackPressed)
        {
            isComboActive = true;
            lastAttackTime = Time.time;
        }
    }

    public void PhysicsUpdate() 
    {

    }

    public void UpdateState()
    {
        HandleInput();
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            AnimatorStateInfo animState = playerState.anim.GetCurrentAnimatorStateInfo(0);

            while (animState.normalizedTime < 0.7f) 
            {
                yield return null;
                animState = playerState.anim.GetCurrentAnimatorStateInfo(0);
            }

            if (isComboActive) 
            {
                isComboActive = false;
                attackCount++;
                PlayNextAttack();
                lastAttackTime = Time.time;
            }
            else if (Time.time - lastAttackTime >= comboResetTime) 
            {
                attackCount = 0;
                playerState.SwitchState(new IdleState(playerState));
                yield break;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }


    private void PlayNextAttack()
    {
        int attackIndex = attackCount % 2; // Spear (0), Sword (1)
        playerState.anim.SetInteger("AttackIndex", attackIndex);

        if (attackIndex == 0 && attackCount > 0) 
        {
            AnimatorStateInfo currentState = playerState.anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Attack_Sword") && currentState.normalizedTime < 0.8f)
            {
                playerState.anim.Play("Attack_Sword", 0, 0.8f); 

            playerState.anim.Play(attackAnimations[attackIndex], 0, 0.25f); 
        }
        else if (attackIndex == 1 && attackCount > 0) 
            playerState.anim.Play(attackAnimations[attackIndex], 0);
        }
        else
        {
            playerState.PlayAnimation(attackAnimations[attackIndex]);
        }
    }

}
