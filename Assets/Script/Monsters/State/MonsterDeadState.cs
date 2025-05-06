using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private MonstersStateMachine enemy;
    //private bool hasMonsterDeadAnimation = false;

    public MonsterDeadState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        
        switch (enemy.enemy.enemyType)
        {
            case EnemyType.Assassin:
               // enemy.animMonster.SetBool("isDead", true);
                enemy.animMonster.Play("Dead");
                //hasMonsterDeadAnimation = true;
                break;
            case EnemyType.Mage:
                //enemy.animMonster.SetBool("isDead", true);
                //enemy.animMonster.SetBool("isDead", true);
                enemy.animMonster.Play("Dead_Mage");
                break;
        }
        //hasMonsterDeadAnimation = true;

        
        
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
