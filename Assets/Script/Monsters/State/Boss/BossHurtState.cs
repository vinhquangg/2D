using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurtState : IMonsterState
{
    private BossStateMachine boss;
    private float bossHurtTimer;

    public BossHurtState(BossStateMachine bossState)
    {
        this.boss = bossState;
    }
    public void EnterState()
    {
        boss.rbBoss.isKinematic = true;
        bossHurtTimer = boss.bossData.hitDuration;
        boss.boss.Knockback(boss.boss.player.position, boss.bossData.knockbackForce);
    }

    public void ExitState()
    {
        boss.rbBoss.isKinematic = false;
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        bossHurtTimer -= Time.deltaTime;
        if(bossHurtTimer <= 0)
        {
            boss.SwitchState(new BossIdleState(boss));
        }
    }

}
