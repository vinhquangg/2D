using UnityEngine;
using System.Collections;
using System;
using TMPro;

public abstract class BaseEnemy : MonoBehaviour,ISaveable
{
    protected Animator anim;
    public MonstersStateMachine monsterState { get; protected set; }
    public BossStateMachine bossState { get; protected set; }
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
    public GameObject soulPrefab;
    public int enemyID;
    public string zoneID;
    public Color originalColor { get; private set; }
    public float hitDuration ;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public float nameVisibleDistance = 3f;
    public SpawnZone assignedZone;
    public float currentDamage { get; set; }
    public float currentAttackMonsterRange { get; set; }
    public float currentHealth { get;  set; }
    public float knockbackForce = 5f;
    public float patrolSpeed = 1f;
    public bool isKnockback = false;
    public bool isBoss = false;
    public bool isDead = false;
    public bool isLoad = false;
    public Transform currentPoint { get; set; }
    private Transform soulSpawnPoint;
    private bool deathHandled = false;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        healthBar = GetComponentInChildren<MonsterSideHealthBar>();
        currentPoint = pointA != null ? pointA.transform : transform;

        if (enemyType == EnemyType.Boss)
        {
            isBoss = true;
            bossState = GetComponent<BossStateMachine>();
            SetupBoss();
        }
        else
        {
            isBoss = false;
            monsterState = GetComponent<MonstersStateMachine>();
            SetupStats();
        }
        soulSpawnPoint = this.transform;
        InitEnemy();
    }

    private void SetupStats()
    {
        if (monsterState == null)
            monsterState = GetComponent<MonstersStateMachine>();

        if (monsterState == null || monsterState.monsterData == null)
            return;

        currentHealth = monsterState.monsterData.maxHealth;
        currentDamage = monsterState.monsterData.attackDamageToPlayer;
        currentAttackMonsterRange = monsterState.monsterData.attackMonsterRange;

        if (healthBar != null)
        {
            healthBar.UpdateHealBar(currentHealth, monsterState.monsterData.maxHealth);
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        SetMonsterNameByType();
    }

    private void SetupBoss()
    {
        if (bossState == null)
            bossState = GetComponent<BossStateMachine>();

        if (bossState == null || bossState.bossData == null)
            return;

        if (bossState != null && bossState.bossData != null)
        {
            currentHealth = bossState.bossData.maxHealth;
            currentDamage = bossState.bossData.attackDamage;
            currentAttackMonsterRange = bossState.bossData.attackRange;

            if (healthBar != null)
            {
                healthBar.UpdateHealBar(currentHealth, bossState.bossData.maxHealth);
            }
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }
    }

    private void InitEnemy()
    {
        if (!isLoad)
        {
            if (isBoss)
            {
                SetupBoss();  
            }
            else
            {
                SetupStats();
            }
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

    protected virtual void SetMonsterNameByType()
    {
        if (monsterName == null) return;
        monsterName.text = enemyType.ToString();
    }

    public virtual void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (isDead || deathHandled) return;

        if (isBoss && bossState.bossCurrentState is BossCastSkillState)
            return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            float maxHealth = isBoss ? bossState.bossData.maxHealth : monsterState.monsterData.maxHealth;
            healthBar.UpdateHealBar(currentHealth, maxHealth);
        }

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));
        StartCoroutine(Knockback(attackerPosition, knockbackForce));

        if (currentHealth <= 0)
        {
            isDead = true;
            deathHandled = true; 

            if (isBoss)
            {
                HandleBossDeath();
            }
            else
            {
                HandleEnemyDeath();
            }
        }
        else
        {
            if (isBoss)
                bossState.SwitchState(new BossHurtState(bossState));
            else
                monsterState.SwitchState(new MonsterHurtState(monsterState));
        }
    }



    protected virtual void HandleBossDeath()
    {
        bossState.SwitchState(new BossDeadState(bossState));

        if (pointA != null) Destroy(pointA);
        if (pointB != null) Destroy(pointB);

        if (EnemySpawnerManager.Instance != null)
    {
        EnemySpawnerManager.Instance.EnemyDied(this);
    }

        Debug.Log("Boss is dead!");
    }

    protected virtual void HandleEnemyDeath()
    {
        monsterState.SwitchState(new MonsterDeadState(monsterState));

        if (pointA != null) Destroy(pointA);
        if (pointB != null) Destroy(pointB);

        if (soulPrefab != null)
        {
            GameObject soulInstance = Instantiate(soulPrefab, soulSpawnPoint.position, Quaternion.identity);
            SoulDrop soulDrop = soulInstance.GetComponent<SoulDrop>();
            if (soulDrop != null)
            {
                soulDrop.SetSoulAmount(monsterState.monsterData.soulDrop);
            }
        }

        if (EnemySpawnerManager.Instance != null)
        {
            EnemySpawnerManager.Instance.EnemyDied(this);
        }

        Debug.Log($"{enemyType} is dead!");
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
    public virtual IEnumerator ChangeColorTemporarily(Color newColor, float duration, float damage)
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
        isDead = false;
        deathHandled = false;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        if (enemyType == EnemyType.Boss)
        {
            SetupBoss();
            bossState?.SwitchState(new BossIdleState(bossState));
        }
        else
        {
            SetupStats();
            monsterState?.SwitchState(new MonsterIdleState(monsterState));
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


        assignedZone = EnemySpawnerManager.Instance.GetZoneByID(zoneID);


        if (healthBar != null)
        {
            healthBar.UpdateHealBar(currentHealth,
                isBoss ? bossState.bossData.maxHealth : monsterState.monsterData.maxHealth);
        }

        if (pointA != null) pointA.transform.position = save.patrolA;
        if (pointB != null) pointB.transform.position = save.patrolB;

        if (currentHealth <= 0)
        {
            isDead = true;
            if (enemyType != EnemyType.Boss)
            {
                if (pointA != null) Destroy(pointA);
                if (pointB != null) Destroy(pointB);
            }
            ObjectPooling.Instance.ReturnToPool(enemyType, gameObject);
            return;
        }

        if (monsterState != null && monsterState.stateFactory.TryGetValue(save.currentState, out var createState))
        {
            monsterState.SwitchState(createState());
        }
        else
        {
            monsterState?.SwitchState(new MonsterPatrolState(monsterState));
        }

        InitEnemy();
    }

    public static BaseEnemy GetEnemyFromTransform(Transform t)
    {
        var enemy = t.GetComponent<BaseEnemy>();
        if (enemy == null)
        {
            enemy = t.GetComponent<BaseBoss>(); 
        }
        return enemy;
    }
}
