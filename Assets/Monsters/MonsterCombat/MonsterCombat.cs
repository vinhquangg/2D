using System;
using System.Collections;
using UnityEngine;

public abstract class MonsterCombat: MonoBehaviour,IMonsterCombat
{
    protected MonstersStateMachine monsterState;
    protected BaseEnemy baseEnemy;
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
    }

    protected void AttackHit(Vector2 attackPosition, float attackRange)
    {

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPosition, attackRange);

        foreach (Collider2D target in hitTargets)
        {

            if (target.CompareTag(playerTag))
            {
                Debug.Log("Tìm thấy đối tượng Player: " + target.name);

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

    public abstract void Attack();

    public abstract void StopAttack();
}
