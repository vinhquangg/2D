using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
   
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitDuration = 0.2f;
    public GameObject floatingDamge;
    public MonsterData monsterData;

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
        monsterData.maxHealth -= damage;
        Debug.Log($"💔 Quái {gameObject.name} bị đánh, máu còn: {monsterData.maxHealth}");

        if (spriteRenderer != null)
        {
            StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration,damage));

        }
        //if (floatingDamge != null)
        //{
        //    GameObject damageText = Instantiate(floatingDamge, transform.position, Quaternion.identity);
        //    damageText.GetComponent<DamgeFloat>().SetFloat(damage, Color.red);
        //}
        else
        {
            Debug.LogError("❌ Không tìm thấy SpriteRenderer để đổi màu!");
        }

        if (monsterData.maxHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator ChangeColorTemporarily(Color newColor, float duration,int damage)
    {
        Debug.Log($"🎨 Đổi màu quái {gameObject.name} thành {newColor}");
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
        if (floatingDamge != null)
        {
            GameObject damageText = Instantiate(floatingDamge, transform.position, Quaternion.identity);
            damageText.GetComponent<DamgeFloat>().SetFloat(damage, transform);
        }
    }

    private void Die()
    {
        Debug.Log("☠ " + gameObject.name + " đã chết!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {

                player.TakeDamage(monsterData.attackDamageToPlayer);
            }
        }
    }
}
