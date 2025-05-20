using System.Collections;
using UnityEngine;

public class PoisonsLordCombat : BossCombat
{
    private Coroutine invincibleCoroutine;
    private float defaultInvincibleTime;
    private bool hasForcedSummon = false;
    private bool canCastSpecial = true;
    private Coroutine currentCastCoroutine;

    protected override void Start()
    {
        base.Start();
        defaultInvincibleTime = invincibleTime;
        StartCoroutine(UseSkillsLoop());
    }

    private IEnumerator UseSkillsLoop()
    {
        while (true)
        {
            boss.ActivatePhaseTwo();

            if (!hasForcedSummon && boss.currentHealth <= boss.bossState.bossData.maxHealth * 0.3f)
            {
                if (currentCastCoroutine != null)
                {
                    StopCoroutine(currentCastCoroutine);
                    currentCastCoroutine = null;
                }
                yield return StartCoroutine(ForceSummon());
                hasForcedSummon = true;
            }

            if (canCastSpecial && !IsCastingSkill)
            {
                int currentPhase = boss.isPhaseTwoActive ? 2 : 1;
                var skillsThisPhase = bossSkillManager.skills.FindAll(s =>
                    boss.isPhaseTwoActive ? (s.skillPhase == 1 || s.skillPhase == 2) : s.skillPhase == 1
                );

                if (skillsThisPhase.Count > 0)
                {
                    var skill = skillsThisPhase[Random.Range(0, skillsThisPhase.Count)];

                    canCastSpecial = false;
                    isInvincible = true;

                    boss.bossState.SwitchState(new BossCastSkillState(boss.bossState));

                    if (currentCastCoroutine != null)
                    {
                        StopCoroutine(currentCastCoroutine);
                    }
                    currentCastCoroutine = StartCoroutine(bossSkillManager.CastSkill(skill));
                    yield return currentCastCoroutine;

                    yield return new WaitForSeconds(skill.specialAbilityCD);

                    canCastSpecial = true;
                    isInvincible = false;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }


    private IEnumerator ForceSummon()
    {
        isInvincible = true;
        canCastSpecial = false;

        boss.bossState.SwitchState(new BossCastSkillState(boss.bossState));

        var summonSkill = bossSkillManager.skills.Find(s => s.skillName == "Summoner");

        if (summonSkill != null)
        {
            bossSkillManager.CurrentSkill = summonSkill;
            yield return bossSkillManager.StartCoroutine(bossSkillManager.CastSkill(summonSkill));
        }
        else
        {
            Debug.LogWarning("Không tìm thấy skill 'Summoner' trong danh sách!");
        }

        yield return new WaitForSeconds(1f); 
        isInvincible = false;
        canCastSpecial = true;
        hasForcedSummon = true;
        //boss.isPhaseTwoActive = false;
    }


    public override void ReceiveDamage(float damage, Vector2 attackerPosition)
    {
        if (boss.bossState != null && bossState.bossCurrentState is BossCastSkillState)
        {
            invincibleTime += 0.5f;
        }
        else
        {
            invincibleTime = defaultInvincibleTime;
        }

        base.ReceiveDamage(damage, attackerPosition);

        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }
        invincibleCoroutine = StartCoroutine(InvincibleCooldown());
    }

    public override void StopAttack()
    {
        base.StopAttack();
        canCastSpecial = false;

        if (currentCastCoroutine != null)
        {
            StopCoroutine(currentCastCoroutine);
            currentCastCoroutine = null;
        }
    }

}
