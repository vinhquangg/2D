using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    protected MonstersStateMachine stateMachine;
    protected Animator anim;
    public MonsterData monsterData;
    public Rigidbody2D rb { get; private set; } 
    public Transform player;
    public Transform textPoint;
    public SpriteRenderer spriteRenderer;
    public Color originalColor { get; private set; }
    public float hitDuration = 0.2f;
    public float detectRange = 20f;
    public float attackRange = 10f;
    public float moveSpeed = 2f;
    public int currentHealth { get; private set; }
    public GameObject floatingDamage;
    public float knockbackForce = 5f;
    public bool isKnockback = false;

    protected virtual void Start()
    {
        stateMachine = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = monsterData.maxHealth;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        //if (spriteRenderer == null)
        //{
        //    if (spriteRenderer == null)
        //    {
        //        Debug.LogError($"❌ {name} không có SpriteRenderer! Hãy kiểm tra trong Inspector.");
        //    }
        //}

        //if (stateMachine == null)
        //{
        //    Debug.LogError($"❌ {name} không có MonstersStateMachine! Hãy gán đúng component.");
        //}

        //if (monsterData == null)
        //{
        //    Debug.LogError($"❌ {name} không có MonsterData! Hãy kiểm tra prefab.");
        //}

        //if (player == null)
        //{
        //    player = GameObject.FindGameObjectWithTag("Player")?.transform;
        //    if (player == null)
        //    {
        //        Debug.LogError($"❌ Không tìm thấy Player! Hãy chắc chắn có đối tượng Player trong scene.");
        //    }
        //}
    }


    public bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }

    //public virtual void TakeDamage(int damage)
    //{
    //    currentHealth -= damage;
    //    Debug.Log($"💔 {name} bị đánh, máu còn: {currentHealth}");

    //    StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));

    //    if (currentHealth <= 0)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    public virtual void TakeDamage(int damage, Vector2 attackerPosition)
    {
        currentHealth -= damage;
        Debug.Log($"💔 {name} bị đánh, máu còn: {currentHealth}");

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));

        //ShowFloatingText(damage);

        // Gọi Knockback
        StartCoroutine(Knockback(attackerPosition, knockbackForce));

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Knockback(Vector2 attackerPosition, float knockbackForce)
    {
        isKnockback = true; // Chặn di chuyển khi knockback
        rb.isKinematic = false; // Đảm bảo knockback hoạt động

        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f); // Thời gian knockback

        isKnockback = false; // Cho phép di chuyển lại
        rb.velocity = Vector2.zero; // Reset vận tốc để tránh trượt
    }



    private IEnumerator ChangeColorTemporarily(Color newColor, float duration, int damage)
    {
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;

        if (floatingDamage != null)
        {
            GameObject floatingText = Instantiate(floatingDamage, textPoint.position, Quaternion.identity);
            floatingText.GetComponent<DamageFloat>().SetText(damage);
            floatingText.GetComponent<DamageFloat>().DestroyAfter(1.5f); // Xóa sau 1.5 giây
        }
    }

    //private void ShowFloatingText(int damage)
    //{
    //    if (floatingDamage != null)
    //    {
    //        GameObject floatingText = Instantiate(floatingDamage, transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
    //        floatingText.GetComponent<DamgeFloat>().SetText(damage, transform);
    //        floatingText.GetComponent<DamgeFloat>().DestroyAfter(1.5f); // Xóa sau 1.5 giây
    //    }
    //}
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
