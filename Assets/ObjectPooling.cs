using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance { get; private set; }

    [System.Serializable]
    public struct PoolConfig
    {
        public EnemyType type;   
        public GameObject prefab;
        public int initialSize;  
    }

    public PoolConfig[] poolConfigs;

    private Dictionary<EnemyType, Queue<GameObject>> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        pools = new Dictionary<EnemyType, Queue<GameObject>>();

        foreach (var cfg in poolConfigs)
        {
            var q = new Queue<GameObject>();
            for (int i = 0; i < cfg.initialSize; i++)
            {
                var go = Instantiate(cfg.prefab);
                go.SetActive(false);
                q.Enqueue(go);
            }
            pools[cfg.type] = q;
        }
    }

    public GameObject Spawn(EnemyType type, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(type))
        {
            //Debug.LogError($"[ObjectPooling] No pool for {type}");
            return null;
        }

        var q = pools[type];
        GameObject go = q.Count > 0
            ? q.Dequeue()
            : Instantiate(GetPrefab(type)); 
        go.transform.SetPositionAndRotation(pos, rot);
        go.SetActive(true);
        return go;
    }

    public void ReturnToPool(EnemyType type, GameObject enemy)
    {
        if (!pools.ContainsKey(type))
        {
            Destroy(enemy);
            return;
        }
        StartCoroutine(ReturnToPoolWithDelay(enemy));
    }

    private IEnumerator ReturnToPoolWithDelay(GameObject enemy)
    {
        var animator = enemy.GetComponent<Animator>();
        if (animator != null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        enemy.SetActive(false);
        pools[enemy.GetComponent<BaseEnemy>().enemyType].Enqueue(enemy);
    }


    private GameObject GetPrefab(EnemyType type)
    {
        foreach (var cfg in poolConfigs)
            if (cfg.type == type) return cfg.prefab;
        //Debug.LogError($"[ObjectPooling] Prefab for {type} not found");
        return null;
    }
}
