using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public int damage = 15;

    void Start()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Enemy"));
        if (enemy != null)
        {
            Debug.Log("Hit");
            BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage, transform.position);
            }
        }

        Destroy(gameObject, 0.5f);
    }
}
