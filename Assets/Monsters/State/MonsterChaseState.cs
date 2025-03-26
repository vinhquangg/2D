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
        enemy.PlayAnimation("Chase");
        enemy.enemy.rb.isKinematic = true;
        Debug.Log("Chasing");
    }

    public void ExitState()
    {
        enemy.animMonster.SetBool("isChase", false);
        enemy.enemy.rb.isKinematic = false;
        enemy.enemy.rb.velocity = Vector2.zero;
        Debug.Log($" Enemy stop chase.");
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        if (enemy.enemy.isKnockback) return; // Không di chuyển nếu đang knockback

        float distance = Vector2.Distance(enemy.transform.position, enemy.enemy.player.position);

        if (distance > enemy.enemy.detectRange)
        {
            enemy.SwitchState(new MonsterIdleState(enemy));
        }
        else if (distance < enemy.enemy.attackRange)
        {
            Debug.Log("Attack Player");
            //enemy.SwitchState(new MonsterAttackState(enemy));
        }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (enemy.enemy.player == null || enemy.enemy.isKnockback) return; // Không đuổi nếu bị knockback

        FlipToPlayer();
        Vector2 direction = (enemy.enemy.player.position - enemy.transform.position).normalized;

        enemy.enemy.rb.MovePosition((Vector2)enemy.enemy.transform.position + direction * enemy.enemy.moveSpeed * Time.fixedDeltaTime);
    }



    private void FlipToPlayer()
    {
        if (enemy.enemy.player == null) return;

        Vector3 scale = enemy.enemy.transform.localScale;

        if (enemy.enemy.player.position.x < enemy.enemy.transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1; 
        }

        else
        {
            scale.x = Mathf.Abs(scale.x); 
        }

        enemy.enemy.transform.localScale = scale; 
    }


}
