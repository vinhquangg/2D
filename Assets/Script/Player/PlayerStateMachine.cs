using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerData playerData;
    public IPlayerState currentState { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public PlayerCombat playerCombat { get; private set; }
    public bool isAttackPressed { get; private set; }

    public Dictionary<string,System.Func<IPlayerState>> stateFactory { get; private set; }
    public string currentStateName => currentState?.GetType().Name;  
    // Start is called before the first frame update

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    void Start()
    {
        stateFactory = new Dictionary<string, System.Func<IPlayerState>>()
        {
            { "IdleState", () => new IdleState(this) },
            { "MoveState", () => new MoveState(this) },
            { "AttackState", () => new AttackState(this) },
            //{ "DeadState", () => new DeadState(this) } 
        };

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

        if (currentState is DeadState) return;
        if (isAttackPressed)
        {
            SwitchState(new AttackState(this));
        }
    }

    public PlayerSaveData GetDefaultPlayerData()
    {
        return new PlayerSaveData(
            Vector3.zero, // startPos
            playerData.maxHealth,
            "IdleState",
            playerCombat.playerEnergy.GetMaxEnergy(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        return new PlayerSaveData(
            transform.position,
            playerCombat.currentHealth,
            currentStateName,
            playerCombat.currentEnergy,
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void LoadFromData(PlayerSaveData data)
    {
        transform.position = data.position;
        playerCombat.currentHealth = data.health;
        playerCombat.currentEnergy = data.energy;

        playerCombat.GetComponent<PlayerHealth>()?.UpdateHealthBarPlayer(data.health, playerData.maxHealth);
        playerCombat.GetComponent<PlayerEnergy>()?.UpdateEnergySlider();

        if (stateFactory.TryGetValue(data.currentState, out var createState))
        {
            SwitchState(createState());
        }
        else
        {
            SwitchState(new IdleState(this));
        }
    }
}
