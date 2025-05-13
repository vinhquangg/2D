using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatrolState : IMonsterState
{
    private BossStateMachine boss;
    private Transform currentPoint;
    private GameObject pointA;
    private GameObject pointB;

    public BossPatrolState(BossStateMachine boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.animBoss.SetBool("isMove", true);
    }

    public void ExitState()
    {
        boss.animBoss.SetBool("isMove", false);
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        Patrol();
    }

    private void Patrol()
    {
        boss.boss.transform.position = Vector2.MoveTowards(
            boss.boss.transform.position,
            boss.boss.currentPoint.position,
            boss.boss.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(boss.boss.transform.position, boss.boss.currentPoint.position) < 0.1f)
        {

            boss.SwitchState(new BossIdleState(boss));
        }

        boss.boss.Flip(boss.boss.currentPoint);

        if (boss.boss.CanSeePlayer())
        {
            boss.SwitchState(new BossAttackState(boss));
        }
    }
}
