using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IMonsterState
{
    private BossStateMachine boss;
    private float idleDuration = 3f;
    private float idleTimer;

    public BossIdleState(BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void EnterState()
    {

        switch (boss.boss.enemyType)
        {
            case EnemyType.Boss:
                //enemy.animMonster.Play("Idle");
                boss.animBoss.Play("PoinsonsLord_Idle");
                break;
            case EnemyType.Boss2:
                //enemy.animMonster.Play("Idle_Mage");
                boss.animBoss.Play("FlameLord_Idle");
                break;

        }
        //boss.animBoss.Play("PoinsonsLord_Idle");
       idleTimer = 0f;
    }

    public void UpdateState()
    {
        if(boss.boss.CanSeePlayer())
        {
            boss.SwitchState(new BossChaseState(boss));
        }
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            boss.boss.currentPoint = (boss.boss.currentPoint == boss.boss.pointA.transform) ?
                                        boss.boss.pointB.transform :
                                        boss.boss.pointA.transform;
            boss.SwitchState(new BossPatrolState(boss));
        }
    }

    public void PhysicsUpdate()
    {
        
    }
    public void ExitState()
    {
       Debug.Log("Exit Idle State");
    }

}
