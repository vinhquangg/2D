﻿using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerData playerData;
    public IPlayerState currentState { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public PlayerCombat playerCombat { get; private set; }
    public bool isAttackPressed { get; set; }

    public string currentZoneID;
    public Dictionary<string, System.Func<IPlayerState>> stateFactory { get; private set; }
    public string currentStateName => currentState?.GetType().Name;
    // Start is called before the first frame update

    void Awake()
    {
        InitializeNull();
    }

    private void InitializeNull(bool force = false)
    {
        if (force || rb == null) rb = GetComponent<Rigidbody2D>();
        if (force || anim == null) anim = GetComponent<Animator>();
        if (force || playerCombat == null) playerCombat = GetComponent<PlayerCombat>();
    }


    void Start()
    {
        stateFactory = new Dictionary<string, System.Func<IPlayerState>>()
        {
            { "IdleState", () => new IdleState(this) },
            { "MoveState", () => new MoveState(this) },
            { "AttackState", () => new AttackState(this) },
            { "DeadState", () => new DeadState(this) } 
        };

        SwitchState(new IdleState(this));
    }

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
        InitializeNull(true);
        Vector3 startPos = Vector3.zero;
        return new PlayerSaveData(
            startPos, 
            playerData.maxHealth,
            "IdleState",
            playerCombat.playerEnergy.GetMaxEnergy(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerData.soul
        );
    }


    public PlayerSaveData GetPlayerSaveData()
    {
        return new PlayerSaveData(
            transform.position,
            playerCombat.currentHealth,
            currentStateName,
            playerCombat.currentEnergy,
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerCombat.currentSoul
        );
    }

    public void LoadFromData(PlayerSaveData data)
    {
        InitializeNull();

        transform.position = data.position;

        if (playerCombat != null)
        {
            playerCombat.currentHealth = data.health;
            playerCombat.currentEnergy = data.energy;
            playerCombat.currentSoul = data.soul;

            var health = playerCombat.GetComponent<PlayerHealth>();
            var energy = playerCombat.GetComponent<PlayerEnergy>();
            var soul = playerCombat.GetComponent<PlayerSoul>();

            if (health != null)
            {
                health.UpdateHealthBarPlayer(playerCombat.currentHealth, playerData.maxHealth);
            }

            if (energy != null)
            {
                energy.UpdateEnergySlider(); 
            }

            if (soul != null)
            {
                soul.SetSoul(playerCombat.currentSoul); 
            }
        }


        if (stateFactory != null && stateFactory.TryGetValue(data.currentState, out var createState))
        {
            SwitchState(createState());
        }
        else
        {
            SwitchState(new IdleState(this));
        }
       

    }
}