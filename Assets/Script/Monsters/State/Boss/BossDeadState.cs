using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : IMonsterState
{
    private BossStateMachine boss;

    public BossDeadState(BossStateMachine bossState)
    {
        this.boss = bossState;
    }
    public void EnterState()
    {
        switch (boss.boss.enemyType)
        {
            case EnemyType.Boss:
                //enemy.animMonster.Play("Idle");
                boss.animBoss.Play("PoinsonsLord_Death");
                break;
            case EnemyType.Boss2:
                //enemy.animMonster.Play("Idle_Mage");
                boss.animBoss.Play("FlameLord_Death");
                break;

        }
    }

    public void ExitState()
    {

    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {

    }
}
