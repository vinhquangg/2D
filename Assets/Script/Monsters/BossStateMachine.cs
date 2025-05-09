using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonstersStateMachine
{
    [Header("Boss Data")]
    public BossData bossData;

    protected override void Awake()
    {
        base.Awake();
        if (bossData == null)
        {
            Debug.LogError("BossData is not assigned.");
        }
    }

    protected override void Start()
    {
        stateFactory = new Dictionary<string, System.Func<IMonsterState>>()
        {
            //{ "BossIdleState", () => new BossIdleState(this) },
            //{ "BossChaseState", () => new BossChaseState(this) },
            //{ "BossAttackState", () => new BossAttackState(this) },
            //{ "BossSummonState", () => new BossSummonState(this) },
            //{ "BossPhase2State", () => new BossPhase2State(this) },
        };

        SwitchState(stateFactory["BossIdleState"]());
    }

    protected override void Update()
    {
        base.Update(); 

    }
}
