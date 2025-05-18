using System.Collections;
using UnityEngine;

public class MageCombat : MonsterCombat
{
    public Transform firePoint;
    public float projectileSpeed = 5f;
    //public float shootCooldown = 5f;  
    public int maxShots = 3;
    private PlayerCombat playerCombat;
    private RangedEnemy rangedEnemy;
    public GameObject projectilePrefab;
    public bool IsInvincible => isInvincible;
    //private float lastShootTime = 0f;
    //private int currentShots = 0;
    private void Start()
    {
        rangedEnemy = GetComponent<RangedEnemy>();
    }

    public override void Attack()
    {
        isAttacking = true;
        //currentShots = 0;

    }

    public override void StopAttack()
    {
        isAttacking = false;
    }

    public void ShootProjectile()
    {
        if (monsterState.enemy.player == null || !monsterState.enemy.player.gameObject.activeInHierarchy)
        {
            monsterState.SwitchState(new MonsterIdleState(monsterState));
            return;
        }

        if (firePoint == null || projectilePrefab == null) return;


        float spacing = 1f;
        float offsetStart = -((maxShots - 1) * spacing) / 2f;

        for (int i = 0; i < maxShots; i++)
        {
            float offset = offsetStart + i * spacing;

 
            Vector3 spawnPos = firePoint.position + new Vector3(offset, 0f, 0f);

            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);


            Vector2 baseDirection = (monsterState.enemy.player.position - firePoint.position).normalized;

            FlipOrRotateProjectile(projectile, baseDirection);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = baseDirection * projectileSpeed;
            }

            // Bật trail nếu có
            TrailRenderer tr = projectile.GetComponent<TrailRenderer>();
            if (tr != null)
            {
                tr.Clear();
                tr.emitting = true;
            }
        }
    }

    private void FlipOrRotateProjectile(GameObject projectile, Vector2 direction)
    {
        SpriteRenderer sr = projectile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = direction.x < 0;
        }
    }

}
