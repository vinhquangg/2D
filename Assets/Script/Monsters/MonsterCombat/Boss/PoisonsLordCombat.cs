using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonsLordCombat : BossCombat
{
    private Coroutine invincibleCoroutine;
    private float defaultInvincibleTime;

    private float specialAbilityCooldownTimer = 0f;
    private bool canCastSpecial = true;
    protected override void Start()
    {
        base.Start();
        defaultInvincibleTime = invincibleTime;
    }
    public override void Attack()
    {
        base.Attack();
        boss.ActivatePhaseTwo();

        if (boss.isPhaseTwoActive)
        {
            if (!summonActivated && canCastSpecial)
            {
                isInvincible = true;
                boss.bossState.SwitchState(new BossCastSkillState(boss.bossState));
                summonActivated = true;
                IsCastingSkill = true;
                canCastSpecial = false;
                specialAbilityCooldownTimer = boss.bossState.bossData.specialAbilityCD;

                StartCoroutine(ResetCastSkillAfterDelay(bossSkillManager.CurrentSkill.castTime));
                StartSpecialCooldown();
            }
        }
        else
        {
            Debug.Log("Boss phase 1 attack!");
        }
    }

    public override void ReceiveDamage(float damage, Vector2 attackerPosition)
    {
        if (boss.bossState != null && bossState.bossCurrentState is BossCastSkillState)
        {
            invincibleTime += 0.5f;  
            Debug.Log("Invincible Time extended: " + invincibleTime);
        }
        else
        {
            invincibleTime = defaultInvincibleTime;  
            Debug.Log("Invincible Time reset to default: " + invincibleTime);
        }

        base.ReceiveDamage(damage, attackerPosition);

        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }
        invincibleCoroutine = StartCoroutine(InvincibleCooldown());
    }

    private void StartSpecialCooldown()
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        cooldownCoroutine = StartCoroutine(SpecialCooldownCoroutine());
    }

    private Coroutine cooldownCoroutine;

    private IEnumerator SpecialCooldownCoroutine()
    {
        canCastSpecial = false;
        yield return new WaitForSeconds(boss.bossState.bossData.specialAbilityCD);
        canCastSpecial = true;
    }

    private IEnumerator ResetCastSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsCastingSkill = false;
        summonActivated = false; 
    }

    //private void UsePoisonSkill()
    //{
    //    // Ví dụ: gọi skill độc từ BossSkillManager
    //    if (bossSkillManager != null)
    //    {
    //        bossSkillManager.CastPoisonCloud();
    //        Debug.Log("PoisonsLord sử dụng skill Poison Cloud!");
    //    }
    //}

    public override void StopAttack()
    {
        base.StopAttack();

        // Thêm logic dừng tấn công nếu cần
    }
}
