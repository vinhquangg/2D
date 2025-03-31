using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinCombat : MonsterCombat
{

    public bool playerInRange = false;
    private PlayerCombat playerCombat;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCombat = other.GetComponent<PlayerCombat>();

            if (playerCombat != null)
            {
                playerInRange = true; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; 
        }
    }

    public override void Attack()
    {
        if (isAttacking || !playerInRange) return;

        isAttacking = true;
        Debug.Log("Assassin bắt đầu tấn công!");
        monsterState.SwitchState(new MonsterAttackState(monsterState));
    }

    public override void StopAttack()
    {
        isAttacking = false;
        Debug.Log("Assassin dừng tấn công!");
    }

    protected override void Die()
    {
        Debug.Log("Assassin đã chết!");
        base.Die();
    }
    public void PerformAttack()
    {
        if (playerInRange)
        {
            Vector2 attackPosition = playerCombat.transform.position; 

            AttackHit(attackPosition, baseEnemy.currentAttackMonsterRange);
        }
        //if (playerInRange && playerCombat != null)
        //{
        //    playerCombat.TakeDamage(baseEnemy.currentDamage); 
        //    Debug.Log("Assassin gây sát thương lên Player!");
        //}
    }

}
