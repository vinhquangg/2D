using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        currentDamage = 10;
        currentAttackMonsterRange = 0.5f;
    }

    public override bool CanSeePlayer()
    {
        return base.CanSeePlayer();
    }

    public override void TakeDamage(int damage, Vector2 attackerPosition)
    {
        StartCoroutine(ChangeColorTemporarily(Color.blue, hitDuration, damage));
    }

    public override IEnumerator Knockback(Vector2 attackerPosition, float knockbackForce)
    {
        return base.Knockback(attackerPosition, knockbackForce);
    }

    public override IEnumerator ChangeColorTemporarily(Color newColor, float duration, int damage)
    {
        return base.ChangeColorTemporarily(newColor, duration, damage);
    }
}
