using UnityEngine;

public class BossCastSkillState : IMonsterState
{
    private BossStateMachine bossState;
    private BossSkillManager bossSkillManager;

    private float skillCooldownTimer = 0f;

    private bool hasStartedCast = false;

    public BossCastSkillState(BossStateMachine bossState)
    {
        this.bossState = bossState;
        this.bossSkillManager = bossState.GetComponent<BossSkillManager>();
    }

    public void EnterState()
    {

        hasStartedCast = false;
        skillCooldownTimer = 0f;

        bossState.boss.Flip(bossState.boss.player.transform);
    }

    public void ExitState()
    {

        bossState.rbBoss.isKinematic = true;
    }

    public void UpdateState()
    {

        if (!hasStartedCast && !bossSkillManager.isCastingSkill)
        {
            bossSkillManager.UseNextSkill();


            if (bossSkillManager.CurrentSkill != null)
                skillCooldownTimer = bossSkillManager.CurrentSkill.castTime;
            else
                skillCooldownTimer = 0f;

            hasStartedCast = true;
        }


        if (skillCooldownTimer > 0)
        {
            skillCooldownTimer -= Time.deltaTime;
        }

        AnimatorStateInfo animInfo = bossState.animBoss.GetCurrentAnimatorStateInfo(0);

        if (skillCooldownTimer <= 0 && !bossSkillManager.isCastingSkill && animInfo.normalizedTime >= 1f)
        {

            bossState.SwitchState(new BossAttackState(bossState));
        }
    }

    public void PhysicsUpdate()
    {

    }
}
