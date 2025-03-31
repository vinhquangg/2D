using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackRange=detectRange = 10f;
        moveSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
