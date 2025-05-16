using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float blinkInterval = 0.2f;

    private SpriteRenderer spriteRenderer;
    private bool isActive = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (isActive)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            var playercomabt = other.GetComponent<PlayerCombat>();
            if (playercomabt != null)
            {
                playercomabt.TakeDamage(damage);
            }
             Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        isActive = false;
    }
}
