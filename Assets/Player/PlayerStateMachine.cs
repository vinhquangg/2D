using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerData playerData;
    public IPlayerState currentState { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public bool isAttackPressed { get; private set; }
    // Start is called before the first frame update

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        SwitchState(new IdleState(this));
    }

    // Update is called once per frame
    public void SwitchState(IPlayerState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType())
            return;

        if (currentState != null)
        {
            currentState.ExitState();
        }
        
        currentState = newState;
        currentState.EnterState();
    }

    void Update()
    {
        isAttackPressed = PlayerInputHandler.instance.playerAction.Attack.WasPressedThisFrame();
        TryAttack();

        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public void PlayAnimation(string animName)
    {
        if (anim == null) return;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash != 0 && !stateInfo.IsName(animName))
        {
            anim.Play(animName);
        }
    }

    public void TryAttack()
    {
        if (isAttackPressed)
        {
            //if (currentState is AttackState attackState)
            //{
            //    attackState.PlayNextAttack();
            //}
            //else
            //{
            //    SwitchState(new AttackState(this));
            //}
            SwitchState(new AttackState(this));
        }
    }
}
