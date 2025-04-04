using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(damage);
                Destroy(gameObject); // Xóa đạn khi va chạm với Player
            }
        }
    }
}
