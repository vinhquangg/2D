using System.Collections;
using UnityEngine;

public class PoisonsLordCombat : BossCombat
{
    private Coroutine invincibleCoroutine;
    private float defaultInvincibleTime;

    private bool canCastSpecial = true;
    private Coroutine cooldownCoroutine;

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
            if (canCastSpecial && !IsCastingSkill)
            {
                int currentPhase = boss.isPhaseTwoActive ? 2 : 1;

                var skillsThisPhase = bossSkillManager.skills.FindAll(s =>
                    boss.isPhaseTwoActive ? (s.skillPhase == 1 || s.skillPhase == 2) : s.skillPhase == 1
                                    

                );

                if (skillsThisPhase.Count > 0)
                {
                    var skill = skillsThisPhase[Random.Range(0, skillsThisPhase.Count)];

                    IsCastingSkill = true;
                    isInvincible = true;

                    boss.bossState.SwitchState(new BossCastSkillState(boss.bossState));

                    yield return bossSkillManager.StartCoroutine(bossSkillManager.CastSkill(skill));

                    yield return new WaitForSeconds(skill.specialAbilityCD);

                    IsCastingSkill = false;
                    isInvincible = false;
                    canCastSpecial = true;
                }
            }
            yield return null;
        }
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
        StopAllCoroutines();
    }
}
