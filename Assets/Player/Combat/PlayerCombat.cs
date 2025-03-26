using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData;
    public LayerMask enemyLayers;
    public SpriteRenderer spriteRenderer;
    public float hitDuration = 0.2f;
    public float invincibleTime = 0.5f;
    private bool isInvincible = false;
    private int currentHealth;

    private void Start()
    {
        currentHealth = playerData.maxHealth;
    }

    public void OnAttackHit(float attackRange)
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(transform.localScale.x * attackRange, 0);
        AttackHit(attackPosition, attackRange);
    }

    public void AttackHit(Vector2 attackPosition, float attackRange)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(playerData.attackDamage, transform.position);
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        StartCoroutine(BecomeInvincible());
        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration));

        CheckHealth();
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

}
