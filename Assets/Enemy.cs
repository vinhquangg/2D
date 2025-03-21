using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitDuration = 0.2f;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("❌ Không tìm thấy SpriteRenderer trên " + gameObject.name);
            }
        }
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"💔 Quái {gameObject.name} bị đánh, máu còn: {health}");

        if (spriteRenderer != null)
        {
            StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration));
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy SpriteRenderer để đổi màu!");
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ChangeColorTemporarily(Color newColor, float duration)
    {
        Debug.Log($"🎨 Đổi màu quái {gameObject.name} thành {newColor}");
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log("☠ " + gameObject.name + " đã chết!");
        Destroy(gameObject);
    }
}
