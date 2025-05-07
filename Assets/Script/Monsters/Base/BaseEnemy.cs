using UnityEngine;
using System.Collections;
using System;
using TMPro;

public abstract class BaseEnemy : MonoBehaviour,ISaveable
{
    protected Animator anim;
    public MonstersStateMachine monsterState { get; private set; }
    //public MonsterData monsterData;
    public Rigidbody2D rb { get; private set; } 
    public Transform player;
    public Transform textPoint;
    public SpriteRenderer spriteRenderer;
    public GameObject floatingDamage;
    public TextMeshProUGUI monsterName;
    public MonsterSideHealthBar healthBar;
    public GameObject pointA;
    public GameObject pointB;
    public EnemyType enemyType;
    public GameObject patrolPointPrefab;
    public int enemyID;
    public string zoneID;
    public Color originalColor { get; private set; }
    public float hitDuration ;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public float nameVisibleDistance = 3f;
    public SpawnZone assignedZone;
    public int currentDamage { get; set; }
    public float currentAttackMonsterRange { get; set; }
    public float currentHealth { get;  set; }
    public float knockbackForce = 5f;
    public float patrolSpeed = 1f;
    public bool isKnockback = false;
    public bool isDead = false;
    public bool isLoad = false;
    public Transform currentPoint { get; set; }


    protected virtual void Start()
    {
        monsterState = GetComponent<MonstersStateMachine>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<MonsterSideHealthBar>();
        currentPoint = pointA.transform;

        InitEnemy();
    }

    private void InitEnemy()
    {
        if (!isLoad)
        {
            currentHealth = monsterState.monsterData.maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        }

        currentDamage = monsterState.monsterData.attackDamageToPlayer;
        currentAttackMonsterRange = monsterState.monsterData.attackMonsterRange;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        SetMonsterNameByType();
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

        //Vector3 scale = transform.localScale;

        if (targetPoint.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        //transform.localScale = scale;

    }

    protected virtual void SetMonsterNameByType()
    {
        if (monsterName == null) return;
        monsterName.text = enemyType.ToString();
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

            if (EnemySpawnerManager.Instance != null)
            {
                EnemySpawnerManager.Instance.EnemyDied(this);
            }

            //Destroy(gameObject, 0.5f);
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

    public virtual void ResetEnemy()
    {
        if (monsterState == null)
            monsterState = GetComponent<MonstersStateMachine>();

        if (monsterState == null || monsterState.monsterData == null)
        {
            return;
        }

        isDead = false;
        currentHealth = monsterState.monsterData.maxHealth;
        if(healthBar !=null)
        {
            healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        }
        monsterState.SwitchState(new MonsterIdleState(monsterState));
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
            zoneID
        );
    }

    public virtual void LoadData(object data)
    {
        EnemySaveData save = data as EnemySaveData;
        if (save == null) return;
        this.enemyID = save.enemyID;
        this.enemyType = save.type;
        zoneID = save.zoneID;
        transform.position = save.position;
        currentHealth = save.health;
        isLoad = true;

        if (healthBar != null)
        {
            healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        }
        assignedZone = EnemySpawnerManager.Instance.GetZoneByID(zoneID);

        if (pointA != null) pointA.transform.position = save.patrolA;
        if (pointB != null) pointB.transform.position = save.patrolB;

        if (currentHealth <= 0)
        {
            isDead = true;
            monsterState.SwitchState(new MonsterDeadState(monsterState));
            if (pointA != null) Destroy(pointA);
            if (pointB != null) Destroy(pointB);
            Destroy(gameObject, 0.5f);
            return;
        }

        if (monsterState.stateFactory.TryGetValue(save.currentState, out var createState))
        {
            monsterState.SwitchState(createState());
        }
        else
        {
            monsterState.SwitchState(new MonsterPatrolState(monsterState));
        }

        InitEnemy(); 
    }
}
