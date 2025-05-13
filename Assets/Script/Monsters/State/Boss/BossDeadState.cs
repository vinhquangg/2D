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
        boss.animBoss.Play("PoinsonsLord_Death");
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
