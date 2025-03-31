using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHurtState : IMonsterState
{
    private MonstersStateMachine enemy;
    private float hurtTimer;

    public MonsterHurtState(MonstersStateMachine monsterState)
    {
        this.enemy = monsterState;
    }

    public void EnterState()
    {
        enemy.rbMonter.isKinematic = true;
        hurtTimer = enemy.enemy.hitDuration;
        enemy.enemy.Knockback(enemy.enemy.player.position, enemy.enemy.knockbackForce);
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
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0)
        {
            enemy.SwitchState(new MonsterIdleState(enemy));
        }

    }
}
