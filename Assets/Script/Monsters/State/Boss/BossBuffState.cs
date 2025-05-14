using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBuffState : IMonsterState
{
    private BossStateMachine boss;
    public BossBuffState(BossStateMachine boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.rbBoss.velocity = Vector2.zero;
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
