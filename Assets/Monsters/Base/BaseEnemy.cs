using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    protected MonstersStateMachine monsterState;
    protected Animator anim;
    //public MonsterData monsterData;
    public Rigidbody2D rb { get; private set; } 
    public Transform player;
    public Transform textPoint;
    public SpriteRenderer spriteRenderer;
    public GameObject floatingDamage;
    public MonsterSideHealthBar healthBar;
    public GameObject pointA;
    public GameObject pointB;
    public EnemyType enemyType;
    public Color originalColor { get; private set; }
    public float hitDuration ;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public int currentDamage { get; set; }
    public float currentAttackMonsterRange { get; set; }
    public int currentHealth { get;  set; }
    public float knockbackForce = 5f;
    public float patrolSpeed = 1f;
    public bool isKnockback = false;
    public bool isDead = false;
    public Transform currentPoint { get; set; }
    protected virtual void Start()
    {
        monsterState = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar= GetComponentInChildren<MonsterSideHealthBar>();
        currentPoint = pointA.transform;
        currentHealth = monsterState.monsterData.maxHealth;
        healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        currentDamage = monsterState.monsterData.attackDamageToPlayer;
        currentAttackMonsterRange = monsterState.monsterData.attackMonsterRange;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

    }


    public virtual bool CanSeePlayer() // chỉ dùng khi làm quái đánh xa hoặc dùng gậy
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }

    public virtual void Flip(Transform targetPoint)
    {
        if (targetPoint == null) return;

        Vector3 scale = transform.localScale;

        if (targetPoint.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1; // Quay mặt qua trái
        }
        else
        {
            scale.x = Mathf.Abs(scale.x); // Quay mặt qua phải
        }

        transform.localScale = scale;
    }



    public virtual void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));

        StartCoroutine(Knockback(attackerPosition, knockbackForce));

        if (currentHealth <= 0)
        {
            monsterState.SwitchState(new MonsterDeadState(monsterState));
            Destroy(gameObject, 0.5f);
        }
        else
        {
            if (monsterState.monsterCurrentState is MonsterAttackState || 
                monsterState.monsterCurrentState is MonsterChaseState || 
                monsterState.monsterCurrentState is MonsterIdleState||
                monsterState.monsterCurrentState is MonsterPatrolState)
            {
                monsterState.SwitchState(new MonsterHurtState(monsterState));
            }
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

}
