using UnityEngine;
using System.Collections.Generic;

public class BlastWave : MonoBehaviour
{
    public LayerMask enemyLayers;
    public int damage = 10; 
    public float radius = 5f;  
    public GameObject lightningPrefab; 
    public int lightningCount = 3; 

    private List<Transform> enemiesInRange = new List<Transform>();

    void Start()
    {

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<MonsterCombat>()?.ReceiveDamage(damage, transform.position);

            enemiesInRange.Add(enemy.transform);
        }

        Invoke(nameof(SpawnLightning), 1f);


        Destroy(gameObject, 1f);
    }

    void SpawnLightning()
    {
        if (enemiesInRange.Count == 0) return;

        List<Transform> selectedEnemies = new List<Transform>(enemiesInRange);

        for (int i = 0; i < lightningCount; i++)
        {
            if (selectedEnemies.Count == 0) break; 

            int index = Random.Range(0, selectedEnemies.Count);
            Transform target = selectedEnemies[index];


            if(target !=null)
            {
                BaseEnemy enemyScript = target.GetComponent<BaseEnemy>();
                if (enemyScript != null && !enemyScript.isDead && enemyScript.currentHealth > 0)
                {
                    Instantiate(lightningPrefab, target.position, Quaternion.identity);
                }
            }
            selectedEnemies.RemoveAt(index);
            
        }
    }

}
