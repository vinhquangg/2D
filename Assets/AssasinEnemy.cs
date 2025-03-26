using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Attack()
    {
        stateMachine.SwitchState(new MonsterChaseState(stateMachine));
    }
}
