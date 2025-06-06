﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : IMonsterState
{
    private BossStateMachine boss;
    private float chaseSpeed;

    public BossChaseState(BossStateMachine boss)
    {
        this.boss = boss;
        chaseSpeed = boss.bossData.moveSpeed;
    }
    public void EnterState()
    {
        boss.animBoss.SetBool("isMove", true);
        switch (boss.boss.enemyType)
        {
            case EnemyType.Boss:
                //enemy.animMonster.Play("Idle");
                boss.animBoss.Play("PoinsonsLord_Move");
                break;
            case EnemyType.Boss2:
                //enemy.animMonster.Play("Idle_Mage");
                boss.animBoss.Play("FlameLord_Move");
                break;

        }

        //boss.animBoss.SetBool("isMove", true);

        Debug.Log("Chasing");
    }

    public void ExitState()
    {
        boss.animBoss.SetBool("isMove", false);
        Debug.Log("Exit Chase State");
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        if (boss.boss.isKnockback) return;
        if (boss.boss.player == null) return;

        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.boss.player.position);

        if (distanceToPlayer > boss.boss.detectRange)
        {
            Vector2 direction = (boss.boss.player.position - boss.transform.position).normalized;
            boss.SwitchState(new BossIdleState(boss));
        }
        else if (distanceToPlayer < boss.bossData.attackRange)
        {
            boss.SwitchState(new BossAttackState(boss));
            return;
        }
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (boss.boss.player == null || boss.boss.isKnockback) return;

        FlipToPlayer();
        Vector2 direction = (boss.boss.player.position - boss.transform.position).normalized;

        boss.boss.rb.MovePosition((Vector2)boss.boss.transform.position + direction * boss.boss.moveSpeed * Time.fixedDeltaTime);
    }

    private void FlipToPlayer()
    {
        boss.boss.Flip(boss.boss.player);
    }
}
