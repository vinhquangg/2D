using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public float detectRange = 1.5f;
    public float chaseRange = 1f;
    public float attackRange = 0.8f;
    public float moveSpeed = 2f;
    public int heal = 50;
    protected IMonsterState currentState;
    public Transform player;

    protected virtual void Start()
    {
        currentState = new MonsterChaseState(this);
    }

    public virtual void Update()
    {
        DetectPlayer();
        currentState.UpdateState();
    }
    public void SwitchState(IMonsterState newState)
    {
        if (currentState.GetType() == newState.GetType()) return;
        currentState = newState;
        currentState.EnterState();
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            Debug.Log("Attack Player");
            return;
            //SwitchState(new MonsterAttackState(this));
        }
        else if (distance < detectRange)
        {
            SwitchState(new MonsterChaseState(this));
        }
        else
        {
            //SwitchState(new MonsterIdleState(this));
            Debug.Log("Idle....");
            return;
        }
    }

    public abstract void Attack(); 
}
