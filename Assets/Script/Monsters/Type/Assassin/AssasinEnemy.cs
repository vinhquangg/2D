using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        if (!isLoad)
        {
            currentDamage = 10;
            currentAttackMonsterRange = 0.5f;
        }
    }

    public override bool CanSeePlayer()
    {
        return base.CanSeePlayer();
    }

    public override void TakeDamage(int damage, Vector2 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);
    }

    public override IEnumerator Knockback(Vector2 attackerPosition, float knockbackForce)
    {
        return base.Knockback(attackerPosition, knockbackForce);
    }

    public override IEnumerator ChangeColorTemporarily(Color newColor, float duration, int damage)
    {
        return base.ChangeColorTemporarily(newColor, duration, damage);
    }

    public override void Flip(Transform player)
    {
        base.Flip(player);
        //if (monsterName != null)
        //{
        //    // Kiểm tra và sửa localScale.x của tên quái để không bị flip
        //    Vector3 currentScale = monsterName.transform.localScale;
        //    monsterName.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        //}
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
