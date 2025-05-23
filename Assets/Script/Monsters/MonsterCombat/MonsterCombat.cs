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

    protected virtual void Awake()
    {
        monsterState = GetComponent<MonstersStateMachine>();
        baseEnemy = GetComponent<BaseBoss>() ?? GetComponent<BaseEnemy>();
        //baseEnemy = GetComponent<BaseEnemy>();
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
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.hit);
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
        if (!gameObject.activeInHierarchy || isInvincible) return;

        if (baseEnemy == null)
        {
            baseEnemy = GetComponent<BaseBoss>() ?? GetComponent<BaseEnemy>();
            if (baseEnemy == null)
            {
                Debug.LogError($"{name} không có BaseEnemy hoặc BaseBoss để nhận damage.");
                return;
            }
        }

        if (baseEnemy.isDead) return;

        var allEnemies = FindObjectsOfType<BaseEnemy>();
        bool isSoloBossScene = (baseEnemy.isBoss);

        StartCoroutine(InvincibleCooldown());

        if (isSoloBossScene)
        {
            try
            {
                bossState.boss.TakeDamage(damage, attackerPosition);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"{name} gặp lỗi khi nhận damage trong boss scene riêng: {ex.Message}");
            }
        }
        else
        {
            baseEnemy.TakeDamage(damage, attackerPosition);
        }
    }


    public static MonsterCombat GetCombatFromTransform(Transform t)
    {
        return t.GetComponent<MonsterCombat>();
    }



    public abstract void Attack();

    public abstract void StopAttack();
}
