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
        boss.animBoss.SetBool("isRun", true);
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
