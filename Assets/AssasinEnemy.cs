using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1f;
        attackRange = 1.5f;
        detectRange = 2.5f;
    }
    public override void Attack()
    {
        stateMachine.SwitchState(new MonsterChaseState(stateMachine));
    }
}
