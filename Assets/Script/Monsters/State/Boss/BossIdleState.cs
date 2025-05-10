using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IMonsterState
{
    private BossStateMachine boss;
    //private float idleDuration = 3f;
    private float idleTimer;

    public BossIdleState(BossStateMachine boss)
    {
        this.boss = boss;
    }

    public void EnterState()
    {
       boss.animBoss.Play("PoinsonsLord_Idle");
       idleTimer = 0f;
    }

    public void UpdateState()
    {
        if(boss.boss.CanSeePlayer())
        {
            boss.SwitchState(new BossChaseState(boss));
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
