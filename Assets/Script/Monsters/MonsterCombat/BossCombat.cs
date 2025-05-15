using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossCombat : MonsterCombat
{
    protected BaseBoss boss;
    protected BossSkillManager bossSkillManager;
    protected bool summonActivated = false;
    public bool IsCastingSkill = false;
    protected virtual void Start()
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

    public override void ReceiveDamage(float damage, Vector2 attackerPosition)
    {
        base.ReceiveDamage(damage, attackerPosition);
    }
}
