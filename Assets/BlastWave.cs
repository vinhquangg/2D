using UnityEngine;
using System.Collections.Generic;

public class BlastWave : MonoBehaviour
{
    public int damage = 10; // Sát thương của sóng xung kích
    public float radius = 5f;  // Bán kính ảnh hưởng
    public GameObject lightningPrefab; // Prefab của tia sét
    public int lightningCount = 3; // Số lượng tia sét

    private List<Transform> enemiesInRange = new List<Transform>();

    void Start()
    {
        // Tìm tất cả kẻ địch trong phạm vi ảnh hưởng
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<BaseEnemy>()?.TakeDamage(damage, transform.position);
            //Debug.Log("Hit Enemy");
            enemiesInRange.Add(enemy.transform);
        }

        // Gọi sét sau 1 giây
        Invoke(nameof(SpawnLightning), 1f);

        // Hủy sóng xung kích sau khi xong hiệu ứng
        Destroy(gameObject, 1f);
    }

    void SpawnLightning()
    {
        if (enemiesInRange.Count == 0) return;

        List<Transform> selectedEnemies = new List<Transform>(enemiesInRange);

        for (int i = 0; i < lightningCount; i++)
        {
            if (selectedEnemies.Count == 0) break; // Không còn kẻ địch nào để chọn

            int index = Random.Range(0, selectedEnemies.Count);
            Transform target = selectedEnemies[index];

            // Kiểm tra nếu kẻ địch còn sống (giả sử BaseEnemy có thuộc tính bool isAlive hoặc kiểm tra bằng cách khác)
            BaseEnemy enemyScript = target.GetComponent<BaseEnemy>();
            if (enemyScript != null && enemyScript.currentHealth > 0) // Sửa lại điều kiện nếu cần
            {
                Instantiate(lightningPrefab, target.position, Quaternion.identity);
            }

            selectedEnemies.RemoveAt(index); // Loại bỏ kẻ địch đã bị đánh sét khỏi danh sách
        }
    }

}
