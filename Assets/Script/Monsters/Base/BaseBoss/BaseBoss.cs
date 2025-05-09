using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoss : BaseEnemy
{
    public BossStateMachine bossState { get; private set; }
    public float specialAbilityCD { get; private set; }
    private float currentSpecialAbilityCD;

    public bool isPhaseTwoActive = false;

    protected override void Start()
    {
        base.Start();
        bossState = GetComponent<BossStateMachine>();
        currentSpecialAbilityCD = 0f;
    }

    public void UseSpecialAbility()
    {
        if (currentSpecialAbilityCD <= 0f)
        {
            Debug.Log("Boss using special ability!");
        }
    }

    public override void TakeDamage(int damage, Vector2 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);

        if (currentHealth <= bossState.bossData.maxHealth * 0.5f && !isPhaseTwoActive)
        {
            //ActivatePhaseTwo(); // Kích hoạt phase 2 nếu máu dưới 50%
        }
    }
}
