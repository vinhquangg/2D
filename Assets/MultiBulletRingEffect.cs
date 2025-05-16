using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBulletRingEffect : MonoBehaviour
{
    public GameObject bulletPrefab;      
    public int bulletsPerRing = 8;       
    public int ringCount = 4;            
    public float initialRadius = 1f;     
    public float maxRadius = 5f;         
    public float expandDuration = 5f;     

    private List<List<GameObject>> rings = new List<List<GameObject>>();
    private float elapsed = 0f;

    void Start()
    {
        SpawnAllRings();
        StartCoroutine(ExpandAndDestroy());
    }

    void SpawnAllRings()
    {
        for (int ringIndex = 0; ringIndex < ringCount; ringIndex++)
        {
            float ringRadius = Mathf.Lerp(initialRadius, maxRadius, (float)ringIndex / (ringCount - 1));
            List<GameObject> ringBullets = new List<GameObject>();

            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * Mathf.PI * 2f / bulletsPerRing;
                Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * ringRadius;
                GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity, transform);
                ringBullets.Add(bullet);
            }

            rings.Add(ringBullets);
        }
    }

    IEnumerator ExpandAndDestroy()
    {
        while (elapsed < expandDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / expandDuration;

            for (int ringIndex = 0; ringIndex < rings.Count; ringIndex++)
            {
                float startRadius = Mathf.Lerp(initialRadius, maxRadius, (float)ringIndex / (ringCount - 1));
                float targetRadius = maxRadius + 2f;
                float currentRadius = Mathf.Lerp(startRadius, targetRadius, t);

                var ringBullets = rings[ringIndex];
                int bulletCount = ringBullets.Count;

                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = i * Mathf.PI * 2f / bulletCount;
                    Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * currentRadius;
                    if (ringBullets[i] != null)
                        ringBullets[i].transform.position = pos;
                }
            }

            yield return null;
        }

        foreach (var ring in rings)
            foreach (var bullet in ring)
                if (bullet != null)
                    Destroy(bullet);

        rings.Clear();
        Destroy(gameObject);
    }
}
