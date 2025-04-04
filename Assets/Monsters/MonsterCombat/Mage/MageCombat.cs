using System.Collections;
using UnityEngine;

public class MageCombat : MonsterCombat
{
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float projectileSpeed = 5f; 
    public float attackCooldown;

    private Coroutine attackCoroutine;

    public override void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public override void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        isAttacking = false;
    }

    private IEnumerator AttackRoutine()
    {
        while (isAttacking)
        {
            ShootProjectile();
            yield return new WaitForSeconds(attackCooldown); 
        }
    }

    private void ShootProjectile()
    {
        if (firePoint == null || projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (monsterState.enemy.player.position - firePoint.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }
    }
}
