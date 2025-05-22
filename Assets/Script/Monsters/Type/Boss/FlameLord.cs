using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLord : BaseBoss
{
    protected override void Start()
    {
        base.Start();
        isBoss = true;
    }

    public override bool CanSeePlayer()
    {
        return base.CanSeePlayer();
    }

    public override void Flip(Transform targetPoint)
    {
        base.Flip(targetPoint);
    }
}
