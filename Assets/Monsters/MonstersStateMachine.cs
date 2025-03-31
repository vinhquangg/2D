 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class MonstersStateMachine : MonoBehaviour
{
    public IMonsterState monsterCurrentState { get; private set; }

    public MonsterData monsterData;
    public Animator animMonster {get; private set; }
    public Rigidbody2D rbMonter { get; private set; }
    public BaseEnemy enemy { get; private set; }


    void Awake()
    {
        rbMonter = GetComponent<Rigidbody2D>();
        animMonster = GetComponent<Animator>();
        enemy = GetComponent<BaseEnemy>();

        //states = new Dictionary<string, IMonsterState>();

        //if (enemy.enemyType == EnemyType.Melee)
        //{
        //    states["Idle"] = new MonsterIdleState(this);
        //    states["Chase"] = new MonsterChaseState(this);
        //    states["Attack"] = new MeleeAttackState(this);
        //}
        //else if (enemy.enemyType == EnemyType.Ranged)
        //{
        //    states["Idle"] = new MonsterIdleState(this);
        //    states["Chase"] = new MonsterChaseState(this);
        //    states["Attack"] = new RangedAttackState(this);
        //}
        //else if (enemy.enemyType == EnemyType.Assassin)
        //{
        //    states["Idle"] = new MonsterIdleState(this);
        //    states["Chase"] = new MonsterChaseState(this);
        //    states["Attack"] = new AssassinAttackState(this);
        //}

        //currentState = states["Idle"];
        //currentState.EnterState();


    }

    void Start()
    {
        SwitchState(new MonsterIdleState(this));
    }

    // Update is called once per frame
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
        //isAttackPressed = PlayerInputHandler.instance.playerAction.Attack.WasPressedThisFrame();
        //TryAttack();

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

    //public void TryAttack()
    //{
    //    if (isAttackPressed)
    //    {
    //        //if (currentState is AttackState attackState)
    //        //{
    //        //    attackState.PlayNextAttack();
    //        //}
    //        //else
    //        //{
    //        //    SwitchState(new AttackState(this));
    //        //}
    //        SwitchState(new AttackState(this));
    //    }
    //}
}
