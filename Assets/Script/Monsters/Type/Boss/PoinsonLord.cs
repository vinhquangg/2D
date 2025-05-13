using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoinsonLord : BaseBoss
{
    protected override void Start()
    {
        base.Start();
        isBoss = true;
    }

    public override void TakeDamage(int damage, Vector2 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);
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

