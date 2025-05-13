using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonsterCombat
{
    private BaseBoss boss;

    private void Start()
    {
        boss = GetComponent<BaseBoss>();
        if (boss == null)
        {
            Debug.LogError("BaseBoss component not found on this GameObject.");
            return;
        }
    }
    public override void Attack()
    {
        isAttacking = true;

        if (boss.isPhaseTwoActive)
        {
            Debug.Log("Boss phase 2 attack!");
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

}
