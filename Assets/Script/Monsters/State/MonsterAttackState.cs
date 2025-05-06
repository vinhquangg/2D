using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MonsterAttackState : IMonsterState
{
    private MonstersStateMachine enemy;
    //private string[] attackAnimation = { "Attack1" };
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

        if (distance > enemy.enemy.detectRange && enemy.enemy.player.gameObject.activeInHierarchy)
        {
            switch (enemy.enemy.enemyType)
            {
                case EnemyType.Assassin:
                    enemy.SwitchState(new MonsterIdleState(enemy));
                    break;
                case EnemyType.Mage:
                    enemy.SwitchState(new MonsterIdleState(enemy));
                    break;

            }

        }
        else if (distance > enemy.enemy.attackRange && enemy.enemy.player.gameObject.activeInHierarchy)
        {
            switch (enemy.enemy.enemyType)
            {
                case EnemyType.Assassin:
                    enemy.SwitchState(new MonsterChaseState(enemy));
                    break;
                case EnemyType.Mage:
                    enemy.SwitchState(new MonsterAttackState(enemy));

                    break;

            }
            //enemy.SwitchState(new MonsterChaseState(enemy));
        }
        else
        {
            //if (Time.time - lastAttackTime >= comboResetTime)
            //{
            //    attackCount = 0;
            AnimatorStateInfo animState = enemy.animMonster.GetCurrentAnimatorStateInfo(0);
            if (animState.normalizedTime >= 1f && !enemy.animMonster.IsInTransition(0))
            {
                PlayNextAttack();
            }
        }

    }

    private void PlayNextAttack()
    {
        //int animationIndex = /*attackCount %*/ attackAnimation.Length;
        FlipToPlayer();
        switch (enemy.enemy.enemyType)
        {
            case EnemyType.Assassin:
                enemy.animMonster.Play("Attack1");
                break;
            case EnemyType.Mage:
                enemy.animMonster.Play("Attack_Mage");
                break;

        }
        lastAttackTime = Time.time;
    }


    private void FlipToPlayer()
    {
        switch (enemy.enemy.enemyType)
        {
            case EnemyType.Mage:
                enemy.enemy.Flip(enemy.enemy.player);
                break;
        }
    }
}
