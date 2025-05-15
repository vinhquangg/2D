using System;
using System.Collections;
using UnityEngine;

public abstract class MonsterCombat: MonoBehaviour,IMonsterCombat
{
    protected MonstersStateMachine monsterState;
    protected BossStateMachine bossState;
    public BaseEnemy baseEnemy { get; private set; }
    protected SpriteRenderer spriteRenderer;
    public float invincibleTime;
    protected bool isInvincible = false;
    protected bool isAttacking = false;
    private string playerTag = "Player";
    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        monsterState = GetComponent<MonstersStateMachine>();
        baseEnemy = GetComponent<BaseEnemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossState = GetComponent<BossStateMachine>();
    }

    protected void AttackHit(Vector2 attackPosition, float attackRange)
    {

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPosition, attackRange);

        foreach (Collider2D target in hitTargets)
        {

            if (target.CompareTag(playerTag))
            {
                PlayerCombat playerCombat = target.GetComponent<PlayerCombat>();
                if (playerCombat != null)
                {
                    playerCombat.TakeDamage(baseEnemy.currentDamage);
                }
            }   
        }
    }
    protected IEnumerator InvincibleCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;

    }

    public virtual void ReceiveDamage(float damage, Vector2 attackerPosition)
    {
        if (isInvincible) return;

        baseEnemy.TakeDamage(damage, attackerPosition);
        StartCoroutine(InvincibleCooldown());
    }


    public abstract void Attack();

    public abstract void StopAttack();
}
