using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();

    }

    public override bool CanSeePlayer()
    {
        return base.CanSeePlayer();
    }

    public override IEnumerator Knockback(Vector2 attackerPosition, float knockbackForce)
    {
        return base.Knockback(attackerPosition, knockbackForce);
    }

    public override IEnumerator ChangeColorTemporarily(Color newColor, float duration, float damage)
    {
        return base.ChangeColorTemporarily(newColor, duration, damage);
    }

    public override void Flip(Transform player)
    {
        base.Flip(player);
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
    }

    public override object SaveData()
    {
        var baseData = base.SaveData() as EnemySaveData;
        return baseData;
    }

    public override void LoadData(object data)
    {
        base.LoadData(data);
        var enemyData = data as EnemySaveData;
    }
}
