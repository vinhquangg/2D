using System.Collections;
using UnityEngine;

public class MageCombat : MonsterCombat
{
    public Transform firePoint;
    public float projectileSpeed = 5f;
    public float shootCooldown = 5f;  
    public int maxShots = 3;
    private PlayerCombat playerCombat;
    private RangedEnemy rangedEnemy;
    //private float lastShootTime = 0f;
    private int currentShots = 0;
    private void Start()
    {
        rangedEnemy = GetComponent<RangedEnemy>();
    }

    public override void Attack()
    {
        isAttacking = true;
        currentShots = 0;

    }

    public override void StopAttack()
    {
        isAttacking = false;
    }


    public void ShootProjectile() 
    {




        if (firePoint == null || rangedEnemy.projectilePrefab == null) return;



        for (int i = 0; i < maxShots; i++)
        {

            Vector2 direction = (monsterState.enemy.player.position - firePoint.position).normalized;

            Vector2 rotatedDirection = Quaternion.Euler(0, 0, 0) * direction;

            GameObject projectile = Instantiate(rangedEnemy.projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = rotatedDirection * projectileSpeed;
            }
        }
    }

}
