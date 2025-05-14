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
    private Dictionary<EnemyType, List<GameObject>> activeObjects;
    private Dictionary<EnemyType, int> reuseIndex;

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
        activeObjects = new Dictionary<EnemyType, List<GameObject>>();
        reuseIndex = new Dictionary<EnemyType, int>();

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
            activeObjects[cfg.type] = new List<GameObject>();
            reuseIndex[cfg.type] = 0;
        }
    }

    public GameObject Spawn(EnemyType type, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(type)) return null;

        GameObject go = null;

        if (pools[type].Count > 0)
        {
            go = pools[type].Dequeue();
        }
        else
        {
            go = ReuseFromActiveList(type);

            if (go == null)
            {
                GameObject prefab = GetPrefab(type);
                if (prefab != null)
                {
                    go = Instantiate(prefab);
                    go.SetActive(false);
                    Debug.LogWarning($"[Pooling] Auto-instantiated new enemy for type {type} (pool expanded).");
                }
                else
                {
                    Debug.LogError($"[Pooling] Prefab for enemy type {type} not found.");
                    return null;
                }
            }
        }

        go.transform.SetPositionAndRotation(pos, rot);
        go.SetActive(true);

        if (!activeObjects[type].Contains(go))
        {
            activeObjects[type].Add(go);
        }

        return go;
    }

    private GameObject ReuseFromActiveList(EnemyType type)
    {
        var list = activeObjects[type];
        if (list.Count == 0) return null;

        int startIndex = reuseIndex[type];

        for (int i = 0; i < list.Count; i++)
        {
            int index = (startIndex + i) % list.Count;
            GameObject candidate = list[index];
            if (candidate != null && candidate.TryGetComponent<BaseEnemy>(out var enemy) && enemy.isDead)
            {
                reuseIndex[type] = (index + 1) % list.Count;
                return candidate;
            }
        }

        return null;
    }

    public void ReturnToPool(EnemyType type, GameObject enemy)
    {
        if (!pools.ContainsKey(type))
        {
            Destroy(enemy);
            return;
        }

        StartCoroutine(ReturnToPoolWithDelay(enemy, type));
    }

    //private IEnumerator ReturnToPoolWithDelay(GameObject enemy, EnemyType type)
    //{
    //    if (enemy.TryGetComponent<BaseEnemy>(out var baseEnemy))
    //    {
    //        if (baseEnemy.isLoad)
    //        {
    //            // Nếu đang trong trạng thái load, tắt ngay
    //            enemy.SetActive(false);

    //            if (!pools[type].Contains(enemy))
    //                pools[type].Enqueue(enemy);

    //            if (activeObjects[type].Contains(enemy))
    //                activeObjects[type].Remove(enemy);

    //            yield break;
    //        }
    //    }

    //    // Nếu không load → delay 1s rồi tắt
    //    yield return new WaitForSeconds(1f);
    //    enemy.SetActive(false);

    //    if (!pools[type].Contains(enemy))
    //        pools[type].Enqueue(enemy);

    //    if (activeObjects[type].Contains(enemy))
    //        activeObjects[type].Remove(enemy);
    //}

    private IEnumerator ReturnToPoolWithDelay(GameObject enemy, EnemyType type)
    {
        if (SaveLoadManager.IsLoading)
        {
            enemy.SetActive(false);

            if (!pools[type].Contains(enemy))
                pools[type].Enqueue(enemy);

            if (activeObjects[type].Contains(enemy))
                activeObjects[type].Remove(enemy);

            yield break;
        }

        if (enemy.TryGetComponent<BaseEnemy>(out var baseEnemy))
        {
            if (baseEnemy.isLoad)
            {
                enemy.SetActive(false);

                if (!pools[type].Contains(enemy))
                    pools[type].Enqueue(enemy);

                if (activeObjects[type].Contains(enemy))
                    activeObjects[type].Remove(enemy);

                yield break;
            }
        }

        yield return new WaitForSeconds(0.8f);
        enemy.SetActive(false);

        if (!pools[type].Contains(enemy))
            pools[type].Enqueue(enemy);

        if (activeObjects[type].Contains(enemy))
            activeObjects[type].Remove(enemy);
    }

    private GameObject GetPrefab(EnemyType type)
    {
        foreach (var cfg in poolConfigs)
            if (cfg.type == type) return cfg.prefab;
        return null;
    }
}
