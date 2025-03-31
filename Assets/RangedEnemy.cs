using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float attackCooldown;

    private float attackCooldownTimer;
    protected override void Start()
    {
        base.Start();
        attackRange=detectRange = 10f;
        moveSpeed = 1f;
        attackCooldownTimer = 0f;
}

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        if(CanSeePlayer() && attackCooldownTimer <=0)
        {
            ShootProjectile();
            attackCooldownTimer = attackCooldown;
        }
    }

    private void ShootProjectile()
    {
        if(projectilePrefab == null)  return;

        GameObject projecttile = Instantiate(projectilePrefab,transform.position,Quaternion.identity);

        Vector2 direction = (player.position- - transform.position).normalized;
        Rigidbody2D rb = projecttile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

}
