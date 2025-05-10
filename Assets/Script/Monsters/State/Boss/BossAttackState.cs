using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : IMonsterState
{
    private BossStateMachine boss;
    private float lastAttackTime;

    public BossAttackState(BossStateMachine boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.animBoss.enabled = true;
        boss.rbBoss.velocity = Vector2.zero;
        boss.rbBoss.isKinematic = true;

        PlayNextAttack();
    }

    public void ExitState()
    {
        boss.rbBoss.isKinematic = false;
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.boss.player.position);
    
        if(distanceToPlayer > boss.bossData.attackRange && boss.boss.player.gameObject.activeInHierarchy)
        {
            boss.SwitchState(new BossChaseState(boss));
            return;
        }
        else
        {
            boss.SwitchState(new BossAttackState(boss));
        }

    }

    private void PlayNextAttack()
    {

        if (boss.boss.isKnockback) return;

        if (boss.boss.player == null) return;

        FlipToPlayer();
        boss.animBoss.Play("PoinsonsLord_Attack");
    }

    private void FlipToPlayer()
    {
        boss.boss.Flip(boss.boss.player);
    }

}
