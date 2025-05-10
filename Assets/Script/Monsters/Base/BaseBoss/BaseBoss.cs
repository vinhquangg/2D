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


    // Override các phương thức không cần thiết
    public override void TakeDamage(int damage, Vector2 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);

        if (currentHealth <= bossState.bossData.maxHealth * 0.5f && !isPhaseTwoActive)
        {
            // Kích hoạt phase 2 nếu máu dưới 50%
        }
    }
}
