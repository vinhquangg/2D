using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    protected MonstersStateMachine monsterState;
    protected Animator anim;
    public MonsterData monsterData;
    public Rigidbody2D rb { get; private set; } 
    public Transform player;
    public Transform textPoint;
    public SpriteRenderer spriteRenderer;
    public EnemyType enemyType;
    public Color originalColor { get; private set; }
    public float hitDuration ;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public int currentDamage { get; set; }
    public float currentAttackMonsterRange { get; set; }
    public int currentHealth { get;  set; }
    public GameObject floatingDamage;
    public float knockbackForce = 5f;
    public bool isKnockback = false;

    protected virtual void Start()
    {
        monsterState = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = monsterData.maxHealth;
        currentDamage = monsterData.attackDamageToPlayer;
        currentAttackMonsterRange = monsterData.attackMonsterRange;

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


    public virtual bool CanSeePlayer() // chỉ dùng khi làm quái đánh xa hoặc dùng gậy
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }

    public virtual void TakeDamage(int damage, Vector2 attackerPosition)
    {
        currentHealth -= damage;
        Debug.Log($"{name} bị đánh, máu còn: {currentHealth}");

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));

        StartCoroutine(Knockback(attackerPosition, knockbackForce));

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            monsterState.SwitchState(new MonsterHurtState(monsterState));
        }
    }

    public virtual IEnumerator Knockback(Vector2 attackerPosition, float knockbackForce)
    {
        isKnockback = true;
        rb.isKinematic = false; 

        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        isKnockback = false; 
        rb.velocity = Vector2.zero; 
    }
    public virtual IEnumerator ChangeColorTemporarily(Color newColor, float duration, int damage)
    {
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;

        if (floatingDamage != null)
        {
            GameObject floatingText = Instantiate(floatingDamage, textPoint.position, Quaternion.identity);
            floatingText.GetComponent<DamageFloat>().SetText(damage);
            floatingText.GetComponent<DamageFloat>().DestroyAfter(1.5f);
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PlayerCombat player = other.GetComponent<PlayerCombat>();
    //        if (player != null)
    //        {

    //            player.TakeDamage(monsterData.attackDamageToPlayer);
    //        }
    //    }
    //}
}
