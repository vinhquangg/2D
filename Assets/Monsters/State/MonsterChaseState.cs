using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IMonsterState
{
    private BaseEnemy enemy;

    public MonsterChaseState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        Debug.Log("Chasing");
    }

    public void ExitState()
    {
        Debug.Log($" Enemy stop chase.");
    }

    public void PhysicsUpdate()
    {

    }
       
    public void UpdateState()
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if(distance > enemy.detectRange)
        {
            Debug.Log("Can't find Player");
            //enemy.SwitchState(new MonsterAttackState(enemy));
        }
        else if ( distance < enemy.detectRange)
        {
            Debug.Log("Attack Player");
            //enemy.SwitchState(new MonsterAttackState(enemy));
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
        enemy.transform.position += (Vector3)direction * enemy.moveSpeed * Time.deltaTime;
    }
}
