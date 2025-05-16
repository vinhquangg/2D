using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossCastSkillState : IMonsterState
{
    private BossStateMachine bossState;
    private BossSkillManager bossSkillManager;
    private float skillCooldownTimer = 0f;
    private bool isCooldown = false;
    public BossCastSkillState(BossStateMachine bossState)
    {
        this.bossState = bossState;
        this.bossSkillManager = bossState.GetComponent<BossSkillManager>();
    }
    public void EnterState()
    {
        if (!isCooldown)
        {
            bossState.boss.Flip(bossState.boss.player.transform);
            bossSkillManager.UseNextSkill();
            skillCooldownTimer = bossSkillManager.CurrentSkill.castTime ;
            isCooldown = true;
        }
        else
        {
            bossState.SwitchState(new BossAttackState(bossState));
        }
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
        if (skillCooldownTimer > 0)
        {
            skillCooldownTimer -= Time.deltaTime;
        }
        else
        {
            isCooldown = false;
        }

        if (bossState.animBoss.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            bossState.SwitchState(new BossAttackState(bossState));
        }
    }
}
