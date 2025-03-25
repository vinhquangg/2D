using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 4f;
    }
    public override void Attack()
    {
        Debug.Log("Attack");
    }
}
