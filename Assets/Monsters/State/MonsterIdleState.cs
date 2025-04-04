using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IMonsterState
{
    private MonstersStateMachine enemy;
    private float idleDuration =3f;
    private float idleTimer;

    public MonsterIdleState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.animMonster.SetBool("isChase", false);
        //enemy.enemy.rb.velocity = Vector2.zero;
        enemy.PlayAnimation("Idle");
        idleTimer = 0f;
    }

    public void UpdateState()
    {
        if (enemy.enemy.CanSeePlayer())
        {
            switch (enemy.enemy.enemyType)
            {
                case EnemyType.Assassin:
                    enemy.SwitchState(new MonsterChaseState(enemy));
                    break;

                case EnemyType.Ranged:
                    enemy.SwitchState(new MonsterAttackState(enemy));
                    break;
            }
        }


        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            enemy.enemy.currentPoint = (enemy.enemy.currentPoint == enemy.enemy.pointA.transform) ? 
                                        enemy.enemy.pointB.transform : 
                                        enemy.enemy.pointA.transform;
            enemy.SwitchState(new MonsterPatrolState(enemy));
        }
    }

    public void PhysicsUpdate() { }

    public void ExitState()
    {

        Debug.Log($"{enemy.name} rời khỏi trạng thái IDLE");
    }
}
