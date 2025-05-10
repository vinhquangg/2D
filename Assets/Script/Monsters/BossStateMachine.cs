using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public IMonsterState bossCurrentState { get; private set; }
    public BossData bossData;
    public Animator animBoss { get; private set; }
    public Rigidbody2D rbBoss { get; private set; }
    public BaseBoss boss { get; private set; }

    public Dictionary<string, System.Func<IMonsterState>> stateFactory;
    public string bossCurrentStateName => bossCurrentState?.GetType().Name;
    public void Awake()
    {
        rbBoss = GetComponent<Rigidbody2D>();
        animBoss = GetComponent<Animator>();
        boss = GetComponent<BaseBoss>();
    }

    public void Start()
    {
        stateFactory = new Dictionary<string, System.Func<IMonsterState>>()
        {
            { "BossIdleState", () => new BossIdleState(this) },
            { "BossChaseState", () => new BossChaseState(this) },
            { "BossAttackState", () => new BossAttackState(this) },
            //{ "BossSummonState", () => new BossSummonState(this) },
            //{ "BossPhase2State", () => new BossPhase2State(this) },
        };

        SwitchState(new BossIdleState(this));
    }

    public void SwitchState(IMonsterState newState)
    {
        if (bossCurrentState != null && bossCurrentState.GetType() == newState.GetType())
            return;

        if (bossCurrentState != null)
        {
            bossCurrentState.ExitState();
        }

        bossCurrentState = newState;
        bossCurrentState.EnterState();
    }

    protected virtual void Update()
    {
        if (bossCurrentState != null)
        {
            bossCurrentState.UpdateState();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (bossCurrentState != null)
        {
            bossCurrentState.PhysicsUpdate();
        }
    }
}

