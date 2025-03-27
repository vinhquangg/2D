using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IMonsterState
{
    private MonstersStateMachine enemy;

    public MonsterAttackState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        Debug.Log("Attack Player");
    }

    public void ExitState()
    {
        Debug.Log("Back");
    }

    public void PhysicsUpdate()
    {
        
    }

    public void UpdateState()
    {
       
    }
}
