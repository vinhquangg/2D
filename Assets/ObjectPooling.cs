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
        enemy.SetActive(false); 

        yield return new WaitForSeconds(2f); 

        foreach (var pool in pools)
        {
            if (pool.Value.Contains(enemy)) continue;

            pool.Value.Enqueue(enemy);
            break;
        }
    }



    private GameObject GetPrefab(EnemyType type)
    {
        foreach (var cfg in poolConfigs)
            if (cfg.type == type) return cfg.prefab;
        return null;
    }
}
