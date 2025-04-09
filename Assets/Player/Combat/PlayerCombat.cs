using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayers;
    public SpriteRenderer spriteRenderer;
    public float hitDuration = 0.2f;
    public float invincibleTime = 0.25f;
    public float currentHealth { get; set; }
    public float currentEnergy { get; set; }
    private PlayerHealth playerHealth;
    public PlayerEnergy playerEnergy { get; private set; }
    private PlayerStateMachine playerState;
    private bool isInvincible = false;

    private void Start()
    {
        playerState = GetComponent<PlayerStateMachine>();
        playerHealth = GetComponent<PlayerHealth>();
        playerEnergy = GetComponent<PlayerEnergy>();
        currentHealth = playerState.playerData.maxHealth;
        playerHealth.UpdateHealthBarPlayer(currentHealth, playerState.playerData.maxHealth);
        currentEnergy = playerEnergy.GetMaxEnergy();
        //playerEnergy.UpdateEnergySlider();
    }

    public void OnAttackHit(float attackRange)
    {

        Vector2 attackPosition = (Vector2)transform.position + new Vector2(transform.localScale.x * attackRange, transform.localScale.y * attackRange);
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
                // Gọi phương thức nhận sát thương
                enemyScript.TakeDamage(playerState.playerData.attackDamage, transform.position);
                playerEnergy.AddEnergy(playerState.playerData.energyPerHit);
                playerEnergy.UpdateEnergySlider();
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        playerHealth.UpdateHealthBarPlayer(currentHealth, playerState.playerData.maxHealth);

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
        playerState.SwitchState(new DeadState(playerState));
        //gameObject.SetActive(false);
    }

}
