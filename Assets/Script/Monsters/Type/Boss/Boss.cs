using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseBoss
{
    protected override void Start()
    {
        base.Start();
        isBoss = true;
    }
}
