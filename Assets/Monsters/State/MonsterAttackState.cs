using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MonsterAttackState : IMonsterState
{
    private MonstersStateMachine enemy;
    private string[] attackAnimation = { "Attack1" };
    //private int attackCount = 0;
    //private float comboResetTime = 0.5f;
    private float lastAttackTime;

    public MonsterAttackState(MonstersStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.animMonster.enabled = true;
        enemy.rbMonter.velocity = Vector2.zero;
        enemy.rbMonter.isKinematic = true;

        //attackCount = 0;
        PlayNextAttack();
        //enemy.PlayAnimation(attackAnimation[0]);
    }

    public void ExitState()
    {
        enemy.rbMonter.isKinematic = false;
    }

    public void PhysicsUpdate()
    {
        
    }

    public void UpdateState()
    {

        float distance = Vector2.Distance(enemy.transform.position, enemy.enemy.player.position);

        if (distance > enemy.enemy.detectRange)
        {
            enemy.SwitchState(new MonsterIdleState(enemy));
        }
        else if (distance > enemy.enemy.attackRange)
        {
            enemy.SwitchState(new MonsterChaseState(enemy));
        }
        else
        {
            //if (Time.time - lastAttackTime >= comboResetTime)
            //{
            //    attackCount = 0;
            //}
            if (Time.time - lastAttackTime >= 0.25f)
            {
                PlayNextAttack();
            }
        }
    }

    private void PlayNextAttack()
    {
        int animationIndex = /*attackCount %*/ attackAnimation.Length;
        enemy.PlayAnimation("Attack1");
        lastAttackTime = Time.time;
    }  
}
