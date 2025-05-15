using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public int damage = 15;
    public LayerMask enemyLayers;

    void Start()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 0.5f, enemyLayers);
        if (enemy != null)
        {
            MonsterCombat enemyScript = enemy.GetComponent<MonsterCombat>();
            if (enemyScript != null)
            {
                enemyScript.ReceiveDamage(damage, transform.position);
            }
        }

        Destroy(gameObject, 0.5f);
    }
}
