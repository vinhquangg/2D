using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonsterCombat
{
    private BaseBoss boss;
    private BossSkillManager bossSkillManager;
    private bool summonActivated = false;
    private void Start()
    {
        boss = GetComponent<BaseBoss>();
        if (boss == null)
        {
            Debug.LogError("BaseBoss component not found on this GameObject.");
            return;
        }
        bossSkillManager = GetComponent<BossSkillManager>();
        if (bossSkillManager == null)
        {
            Debug.LogError("SkillManager component not found on this GameObject.");
            return;
        }
    }
    public override void Attack()
    {
        isAttacking = true;

        if (boss.isPhaseTwoActive)
        {
            if (!summonActivated)
            {
                boss.bossState.SwitchState(new BossCastSkillState(boss.bossState));
                summonActivated = true;
                InvincibleCooldown();
            }
        }
        else
        {
            Debug.Log("Boss phase 1 attack!");
        }

        AttackHit(transform.position, 2.5f);
    }

    public override void StopAttack()
    {
        isAttacking = false;
    }

    public void ResetSummon()
    {
        summonActivated = false;
    }

}
