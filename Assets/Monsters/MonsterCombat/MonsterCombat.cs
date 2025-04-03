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

    //protected void AttackHit(Vector2 attackPosition, float attackRange)
    //{
    //    Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPosition, attackRange);
    //    Debug.Log("Tìm thấy " + hitTargets.Length + " đối tượng trong phạm vi tấn công.");

    //    foreach (Collider2D target in hitTargets)
    //    {
    //        Debug.Log("Tìm thấy đối tượng: " + target.name);

    //        if (target.CompareTag(playerTag))
    //        {
    //            Debug.Log("Đã phát hiện Player!");

    //            PlayerCombat playerCombat = target.GetComponent<PlayerCombat>();
    //            if (playerCombat != null)
    //            {
    //                playerCombat.TakeDamage(baseEnemy.currentDamage);
    //            }
    //        }
    //    }
    //}


    protected void AttackHit(Vector2 attackPosition, float attackRange)
    {
        // Kiểm tra tất cả các đối tượng trong phạm vi va chạm
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPosition, attackRange);

        // Duyệt qua tất cả các đối tượng trong phạm vi
        foreach (Collider2D target in hitTargets)
        {
            // Kiểm tra nếu đối tượng là Player
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

    protected virtual void Die() 
    {
        gameObject.SetActive(false);
    }

    public abstract void Attack();

    public abstract void StopAttack();
}
