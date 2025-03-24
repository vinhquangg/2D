using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersStateMachine : MonoBehaviour
{
    public MonsterData monsterData;
    public IMonterState monterCurrentState { set; private get; }
    public Rigidbody2D rbMonster { set; private get; }
    public Animator animMonster { set; private get; }


    void Awake()
    {
        rbMonster = GetComponent<Rigidbody2D>();
        animMonster = GetComponent<Animator>();
    }

    void Start()
    {
        //SwitchState(new IdleState(this));
    }

    // Update is called once per frame
    public void SwitchState(IMonterState newState)
    {
        if (monterCurrentState != null && monterCurrentState.GetType() == newState.GetType())
            return;

        if (monterCurrentState != null)
        {
            monterCurrentState.ExitState();
        }

        monterCurrentState = newState;
        monterCurrentState.EnterState();
    }

    void Update()
    {
        //isAttackPressed = PlayerInputHandler.instance.playerAction.Attack.WasPressedThisFrame();
        //TryAttack();

        if (monterCurrentState != null)
        {
            monterCurrentState.UpdateState();
        }
    }

    void FixedUpdate()
    {
        monterCurrentState?.PhysicsUpdate();
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
