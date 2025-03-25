using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IMonsterState
{
    private MonstersStateMachine enemy;

    public MonsterIdleState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.animMonster.SetBool("isChase", false);
        enemy.enemy.rb.velocity = Vector2.zero;
        enemy.PlayAnimation("Idle"); 
    }

    public void UpdateState()
    {
        if (enemy.enemy.CanSeePlayer())
        {
            enemy.SwitchState(new MonsterChaseState(enemy));
        }
    }

    public void PhysicsUpdate() { }

    public void ExitState()
    {
        Debug.Log($"{enemy.name} rời khỏi trạng thái IDLE");
    }
}
