using System.Collections;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine playerState;
    private PlayerCombat playerCombat;
    private int attackCount = 0;
    private string[] attackAnimations = { "Attack_Spear", "Attack_Sword" };
    private float comboResetTime = 0.4f;
    private float lastAttackTime;
    private bool isComboActive = false;

    public AttackState(PlayerStateMachine playerState)
    {
        this.playerState = playerState;
        this.playerCombat = playerState.GetComponent<PlayerCombat>(); // Lấy PlayerCombat từ Player
    }

    public void EnterState()
    {
        playerState.anim.speed = 1.6f;
        playerState.anim.enabled = true;
        playerState.rb.velocity = Vector2.zero;
        attackCount = 0;
        isComboActive = false;
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
        playerState.anim.speed = 1.0f;
    }

    public void HandleInput()
    {
        if (Time.time - lastAttackTime < comboResetTime && playerState.isAttackPressed)
        {
            isComboActive = true;
            lastAttackTime = Time.time;
        }
    }

    public void PhysicsUpdate() { }

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
        int attackIndex = attackCount % 2;
        if (attackCount < 5)
        {
            attackIndex = attackCount % 2;
        }
        else
        {
            attackIndex = Random.Range(0, 2);
        }
        playerState.anim.SetInteger("AttackIndex", attackIndex);
        playerState.anim.Play(attackAnimations[attackIndex], 0);

        //Vector2 attackPosition = (Vector2)playerState.transform.position + new Vector2(playerState.transform.localScale.x * 1.2f, 0);
        //playerCombat.AttackHit(attackPosition, 1.2f);
    }
}
