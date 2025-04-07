using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrolState : IMonsterState
{
    private MonstersStateMachine enemy;
    private Transform currentPoint;
    private GameObject pointA;
    private GameObject pointB;
    public MonsterPatrolState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.animMonster.SetBool("isRun", true);

        switch (enemy.enemy.enemyType)
        {
            case EnemyType.Assassin:
                    enemy.animMonster.Play("Run");
                break;
            case EnemyType.Ranged:
                enemy.animMonster.Play("Walk_Mage");
                break;

        }
        //enemy.PlayAnimation("Run");
    }

    public void ExitState()
    {
        enemy.animMonster.SetBool("isRun", false);
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
        enemy.enemy.transform.position = Vector2.MoveTowards(
            enemy.enemy.transform.position, 
            enemy.enemy.currentPoint.position, 
            enemy.enemy.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(enemy.enemy.transform.position, enemy.enemy.currentPoint.position) < 0.1f)
        {
            
            enemy.SwitchState(new MonsterIdleState(enemy));
        }
        enemy.enemy.Flip(enemy.enemy.currentPoint);

        if (enemy.enemy.CanSeePlayer())
        {
            enemy.SwitchState(new MonsterAttackState(enemy));
        }   
    }
}
