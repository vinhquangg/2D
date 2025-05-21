using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager instance;
    public ItemData healthPotion;
    public ItemData energyPotion;
    public BossCombat boss; 
    public SpawnZone[] allZones;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("[Cheat] Thêm 5 máu và 5 năng lượng.");
            InventoryManager.Instance.AddItem(healthPotion, 5);
            InventoryManager.Instance.AddItem(energyPotion, 5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            int bossLayer = LayerMask.NameToLayer("Boss");
            BaseEnemy bossInScene = null;

            foreach (var enemy in FindObjectsOfType<BaseEnemy>(true)) 
            {
                Debug.Log($"[Cheat Debug] Found: {enemy.name}, layer = {LayerMask.LayerToName(enemy.gameObject.layer)}, active = {enemy.gameObject.activeInHierarchy}, enemyType = {enemy.enemyType}");

                if (enemy.gameObject.layer == bossLayer &&
                    enemy.gameObject.activeInHierarchy &&
                    enemy.enemyType == EnemyType.Boss)
                {
                    bossInScene = enemy;
                    break;
                }
            }

            if (bossInScene != null)
            {
                bossInScene.currentHealth /= 2f;
                bossInScene.healthBar.UpdateHealBar(bossInScene.currentHealth, bossInScene.bossState.bossData.maxHealth);
                Debug.Log("[Cheat] Boss còn 50% máu.");
            }
            else
            {
                Debug.LogWarning("[Cheat] Không tìm thấy boss active trên layer Boss.");
            }
        }



        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (var zone in allZones)
            {
                if (zone != null)
                {
                    zone.ForceClearZone(); 
                }
            }

            Debug.Log("[Cheat] Đã đánh dấu tất cả zone là clear.");
        }
    }
}
