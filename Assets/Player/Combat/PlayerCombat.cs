using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData; // Chứa damage nhân vật
    public LayerMask enemyLayers;
    public SpriteRenderer spriteRenderer;
    public float hitDuration = 0.2f; // Thời gian đổi màu khi trúng đòn
    public float invincibleTime = 0.5f; // Khoảng thời gian không thể nhận damage sau khi bị đánh
    private bool isInvincible = false; // Tránh nhận damage liên tục

    public void OnAttackHit(float attackRange) // Nhận phạm vi từ Animation Event
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(transform.localScale.x * attackRange, 0);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        Debug.Log($"🗡 Tấn công tại {attackPosition}, phạm vi {attackRange}");
        Debug.Log($"🔍 Số lượng quái trúng: {hitEnemies.Length}");

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"💥 Trúng quái: {enemy.name}");

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(playerData.attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Nếu đang trong thời gian bất tử, bỏ qua

        playerData.maxHealth -= damage; // Trừ máu
        Debug.Log($"💔 {gameObject.name} bị đánh, máu còn: {playerData.maxHealth}");

        StartCoroutine(BecomeInvincible());
        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration));

        CheckHealth(); // Kiểm tra máu để xử lý nếu chết
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
        else
        {
            Debug.LogError("❌ Không tìm thấy SpriteRenderer để đổi màu!");
        }
    }

    private void CheckHealth()
    {
        if (playerData.maxHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"☠️ {gameObject.name} đã chết!");
        // Gọi animation chết hoặc xử lý respawn ở đây
        gameObject.SetActive(false);
    }
}
