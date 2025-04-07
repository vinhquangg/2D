using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IMonsterState
{
    private MonstersStateMachine enemy;
    private float chaseSpeed;

    public MonsterChaseState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
        chaseSpeed = enemy.enemy.moveSpeed;
    }

    public void EnterState()
    {
        enemy.animMonster.SetBool("isChase", true);
        enemy.animMonster.Play("Chase");
        Debug.Log("Chasing");
    }

    public void ExitState()
    {
        enemy.animMonster.SetBool("isChase", false);
        Debug.Log($" Enemy stop chase.");
    }

    public void PhysicsUpdate()
    {
        
    }

    public void UpdateState()
    {
        if (enemy.enemy.isKnockback) return;

        float distance = Vector2.Distance(enemy.transform.position, enemy.enemy.player.position);

        if (distance > enemy.enemy.detectRange)
        {
            enemy.SwitchState(new MonsterIdleState(enemy));
        }
        else if (distance < enemy.enemy.attackRange)
        {
            enemy.SwitchState(new MonsterAttackState(enemy));
        }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (enemy.enemy.player == null || enemy.enemy.isKnockback ) return;

        FlipToPlayer();
        Vector2 direction = (enemy.enemy.player.position - enemy.transform.position).normalized;

        enemy.enemy.rb.MovePosition((Vector2)enemy.enemy.transform.position + direction * enemy.enemy.moveSpeed * Time.fixedDeltaTime);
    }

    private void FlipToPlayer()
    {
        enemy.enemy.Flip(enemy.enemy.player);
    }
}
