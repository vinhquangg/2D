using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastSkillState : IMonsterState
{
    private BossStateMachine bossState;
    private BossSkillManager bossSkillManager;
    public BossCastSkillState(BossStateMachine bossState)
    {
        this.bossState = bossState;
        this.bossSkillManager = bossState.GetComponent<BossSkillManager>();
    }
    public void EnterState()
    {
        bossState.rbBoss.velocity = Vector2.zero;
        bossState.rbBoss.isKinematic = true;
        bossSkillManager.UseNextSkill();
    }

    public void ExitState()
    {
        bossState.rbBoss.isKinematic = true;
    }

    public void PhysicsUpdate()
    {

    }

    public void UpdateState()
    {
        if (bossState.animBoss.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            bossState.SwitchState(new BossAttackState(bossState));
        }
    }
}
