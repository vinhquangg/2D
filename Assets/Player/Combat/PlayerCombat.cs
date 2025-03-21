using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData; // Chứa damage nhân vật
    public LayerMask enemyLayers;

    public void OnAttackHit(float attackRange) // Nhận phạm vi từ Animation Event
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(transform.localScale.x * attackRange, 0);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange * 0.5f, enemyLayers);

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
            else
            {
                Debug.LogError("❌ Không tìm thấy script Enemy trên " + enemy.name);
            }
        }
    }
}
