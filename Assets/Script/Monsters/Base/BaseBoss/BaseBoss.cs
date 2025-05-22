using UnityEngine;

public abstract class BaseBoss : BaseEnemy
{
    //public BossStateMachine bossState { get; protected set; }
    public float specialAbilityCD { get; private set; }
    private float currentSpecialAbilityCD;
    public bool isPhaseTwoActive = false;
    protected override void Start()
    {
        base.Start();

        if (currentPoint == null && pointA != null)
        {
            currentPoint = pointA.transform;
        }

        floatingDamage = null;
        textPoint = null;
    }

    protected override void HandleBossDeath()
    {
        base.HandleBossDeath();
    }

    public override bool CanSeePlayer()
    {
        return base.CanSeePlayer();
    }

    public override void Flip(Transform targetPoint)
    {
        base.Flip(targetPoint);
    }

    public virtual void ActivatePhaseTwo()
    {

        if (currentHealth <= bossState.bossData.maxHealth * 0.5f && !isPhaseTwoActive)
        {
            isPhaseTwoActive = true;
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, bossState.bossData.maxHealth);
        healthBar.UpdateHealBar(currentHealth, bossState.bossData.maxHealth);
    }

    public override void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null && bossState?.bossData != null)
        {
            healthBar.UpdateHealBar(currentHealth, bossState.bossData.maxHealth);
        }

        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration, damage));
        StartCoroutine(Knockback(attackerPosition, knockbackForce));

        if (currentHealth <= 0)
        {
            isDead = true;
            HandleBossDeath();
        }
        else
        {
            bossState?.SwitchState(new BossHurtState(bossState));
        }
    }

}
