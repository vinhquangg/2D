using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonsterCombat
{
    private BaseBoss boss;
    private BossSummoner summoner;
    private bool summonActivated = false;
    private void Start()
    {
        boss = GetComponent<BaseBoss>();
        if (boss == null)
        {
            Debug.LogError("BaseBoss component not found on this GameObject.");
            return;
        }
        summoner = GetComponent<BossSummoner>();
        if (summoner == null)
        {
            Debug.LogError("BossSummoner component not found on this GameObject.");
            return;
        }
    }
    public override void Attack()
    {
        isAttacking = true;

        if (boss.isPhaseTwoActive)
        {
            if (!summonActivated)
            {
                summoner?.SummonEnemies();
                summonActivated = true;  
            }
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

    public void ResetSummon()
    {
        summonActivated = false;
    }

}
