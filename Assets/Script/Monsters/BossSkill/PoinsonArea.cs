using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    public float duration = 5f;
    public float tickInterval = 1f;
    public float damagePerTick = 5f;

    private HashSet<GameObject> playersInArea = new HashSet<GameObject>();

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
        StartCoroutine(DamageOverTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);
            foreach (var player in playersInArea)
            {
                if (player != null)
                {
                    player.GetComponent<PlayerCombat>().TakeDamage(damagePerTick);
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.hit);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInArea.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInArea.Remove(other.gameObject);
        }
    }
}
