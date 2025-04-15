using UnityEngine;
using System.Collections;
using System;

public abstract class BaseEnemy : MonoBehaviour,ISaveable
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
    public string enemyID;
    public Color originalColor { get; private set; }
    public float hitDuration ;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    //public SpawnZone assignedZone;
    public int currentDamage { get; set; }
    public float currentAttackMonsterRange { get; set; }
    public float currentHealth { get;  set; }
    public float knockbackForce = 5f;
    public float patrolSpeed = 1f;
    public bool isKnockback = false;
    public bool isDead = false;
    private bool isLoaded = false;
    public Transform currentPoint { get; set; }
    protected virtual void Start()
    { 
        
        monsterState = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar= GetComponentInChildren<MonsterSideHealthBar>();
        currentPoint = pointA.transform;
        if(!isLoaded)
        {
            currentHealth = monsterState.monsterData.maxHealth;
        }
        healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        currentDamage = monsterState.monsterData.attackDamageToPlayer;
        currentAttackMonsterRange = monsterState.monsterData.attackMonsterRange;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

    }


    public virtual bool CanSeePlayer() 
    {
        if (player == null) return false;

        if (player.gameObject.activeInHierarchy && Vector2.Distance(transform.position, player.position) < detectRange)
        {
            return true;
        }

        return false;
    }

    public virtual void Flip(Transform targetPoint)
    {
        if (targetPoint == null) return;

        Vector3 scale = transform.localScale;

        if (targetPoint.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
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
            isDead = true; 


            monsterState.SwitchState(new MonsterDeadState(monsterState));

            if (pointA != null) Destroy(pointA);
            if (pointB != null) Destroy(pointB);

            //if (EnemySpawnerManager.Instance != null)
            //{
            //    EnemySpawnerManager.Instance.EnemyDied(this);
            //}

            Destroy(gameObject, 0.5f);
        }
        else
        {

            if (monsterState.monsterCurrentState is MonsterAttackState ||
                monsterState.monsterCurrentState is MonsterChaseState ||
                monsterState.monsterCurrentState is MonsterIdleState ||
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

    public virtual object SaveData()
    {
        return new EnemySaveData(
            enemyID,
            enemyType,
            transform.position,
            currentHealth,
            monsterState?.monsterCurrentStateName,
            pointA?.transform.position ?? Vector3.zero,
            pointB?.transform.position ?? Vector3.zero,
            "" // Nếu có zoneID, bạn có thể thêm vào đây
        );
    }

public virtual void LoadData(object data)
{
    EnemySaveData save = data as EnemySaveData;
    if (save == null) return;

    this.enemyID = save.enemyID;

    // Debug load data
    Debug.Log("Loaded Data:");
    Debug.Log("Health: " + save.health);
    Debug.Log("Max Health: " + monsterState.monsterData.maxHealth);

    transform.position = save.position;
    currentHealth = save.health;
    isLoaded = true;

    // Kiểm tra lại giá trị health sau khi load
    Debug.Log("Current Health after Load: " + currentHealth);

    // Update healthBar if necessary
    if (healthBar != null)
    {
        healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
    }

    pointA.transform.position = save.patrolA;
    pointB.transform.position = save.patrolB;

    if (monsterState.stateFactory.TryGetValue(save.currentState, out var createState))
    {
        monsterState.SwitchState(createState());
    }
    else
    {
        monsterState.SwitchState(new MonsterPatrolState(monsterState));
    }

    // Kiểm tra xem quái có chết không
    if (currentHealth <= 0)
    {
        isDead = true;
        monsterState.SwitchState(new MonsterDeadState(monsterState));

        if (pointA != null) Destroy(pointA);
        if (pointB != null) Destroy(pointB);

        Destroy(gameObject, 0.5f);
    }
}



}
