using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private MonstersStateMachine enemy;
    private bool hasMonsterDeadAnimation = false;

    public MonsterDeadState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        if(!hasMonsterDeadAnimation)
        {
            enemy.animMonster.SetBool("isDead", true);
            enemy.PlayAnimation("Dead");
            //hasMonsterDeadAnimation = true;
            //enemy.animMonster.SetBool("isDead", false);
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
