using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        attackRange = 10f;
        detectRange = 15f;
        moveSpeed = 1f;
    }

    

}
