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


    void Awake()
    {
        rbMonter = GetComponent<Rigidbody2D>();
        animMonster = GetComponent<Animator>();
        enemy = GetComponent<BaseEnemy>();
    }

    void Start()
    {
        //switch (enemy.enemyType)
        //{
        //    case EnemyType.Assassin:
        //        SwitchState(new Mon)
        //}

        SwitchState(new MonsterIdleState(this));
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

    public void PlayAnimation(string animName)
    {
        if (animMonster == null) return;

        AnimatorStateInfo stateInfo = animMonster.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash != 0 && !stateInfo.IsName(animName))
        {
            animMonster.Play(animName);
        }
    }

}