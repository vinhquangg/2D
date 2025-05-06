using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class MonstersStateMachine : MonoBehaviour
{
    public IMonsterState monsterCurrentState { get; private set; }

    public MonsterData monsterData;
    public Animator animMonster { get; private set; }
    public Rigidbody2D rbMonter { get; private set; }
    public BaseEnemy enemy { get; private set; }

    public Dictionary<string, System.Func<IMonsterState>> stateFactory;
    public string monsterCurrentStateName => monsterCurrentState?.GetType().Name;



    void Awake()
    {
        rbMonter = GetComponent<Rigidbody2D>();
        animMonster = GetComponent<Animator>();
        enemy = GetComponent<BaseEnemy>();
    }

    void Start()
    {
        stateFactory = new Dictionary<string, System.Func<IMonsterState>>()
        {
            { "MonsterIdleState", () => new MonsterIdleState(this) },
            { "MonsterChaseState", () => new MonsterChaseState(this) },
            { "MonsterAttackState", () => new MonsterAttackState(this) },
            { "MonsterHurtState", () => new MonsterHurtState(this) },
            { "MonsterDeadState", () => new MonsterDeadState(this) },
            { "MonsterPatrolState", () => new MonsterPatrolState(this) }, 
        };


        switch (enemy.enemyType)
        {
            case EnemyType.Assassin:
                SwitchState(new MonsterPatrolState(this));
                break;
            case EnemyType.Mage:
                SwitchState(new MonsterPatrolState(this));
                break;
        }

        //SwitchState(new MonsterPatrolState(this));
    }
    public void SwitchState(IMonsterState newState)
    {
        if (monsterCurrentState != null && monsterCurrentState.GetType() == newState.GetType())
            return;

        if (monsterCurrentState != null)
        {
            monsterCurrentState.ExitState();
        }

        monsterCurrentState = newState;
        monsterCurrentState.EnterState();
    }

    void Update()
    {
        if (monsterCurrentState != null)
        {
            monsterCurrentState.UpdateState();
        }
    }

    void FixedUpdate()
    {
        monsterCurrentState?.PhysicsUpdate();
    }
}