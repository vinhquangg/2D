using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    protected MonstersStateMachine stateMachine;
    protected Animator anim;
    public MonsterData monsterData;
    public Rigidbody2D rb { get; private set; } 
    public Transform player;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitDuration = 0.2f;
    public float detectRange = 20f;
    public float attackRange = 10f;
    public float moveSpeed = 2f;
    private int currentHealth;
    public GameObject floatingDamage;

    protected virtual void Start()
    {
        stateMachine = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = monsterData.maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError($"❌ {name} không có SpriteRenderer! Hãy kiểm tra trong Inspector.");
            }
        }

        if (stateMachine == null)
        {
            Debug.LogError($"❌ {name} không có MonstersStateMachine! Hãy gán đúng component.");
        }

        if (monsterData == null)
        {
            Debug.LogError($"❌ {name} không có MonsterData! Hãy kiểm tra prefab.");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError($"❌ Không tìm thấy Player! Hãy chắc chắn có đối tượng Player trong scene.");
            }
        }
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }


    public bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"💔 {name} bị đánh, máu còn: {currentHealth}");

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ChangeColorTemporarily(Color newColor, float duration, int damage)
    {
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;

        if (floatingDamage != null)
        {
            GameObject damageText = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            damageText.GetComponent<DamgeFloat>().SetFloat(damage, transform);
        }
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

    public abstract void Attack();
}
