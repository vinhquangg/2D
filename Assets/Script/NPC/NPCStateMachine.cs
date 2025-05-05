using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public INPCState npcCurrentState { get; private set; }
    public DialogueData npcData;
    public Animator animNPC { get; private set; }
    public Rigidbody2D rbNPC { get; private set; }
    public Dictionary<string, System.Func<INPCState>> stateFactory;

    private void Awake()
    {
        InitializeNull();
        stateFactory = new Dictionary<string, System.Func<INPCState>>()
        {
            { "Idle", () => new NPCIdleState(this) },
            //{ "Walk", () => new NPCWalkState(this) },
            //{ "Talk", () => new NPCTalkState(this) },
            //{ "Interact", () => new NPCInteractState(this) }
        };
    }
    private void InitializeNull()
    {
        if (rbNPC == null) rbNPC = GetComponent<Rigidbody2D>();
        if(animNPC == null) animNPC = GetComponent<Animator>();
    }

    public void SwitchState(INPCState newState)
    {
        if (npcCurrentState != null && npcCurrentState.GetType() == newState.GetType())
            return;

        if (npcCurrentState != null)
        {
            npcCurrentState.ExitState();
        }

        npcCurrentState = newState;
        npcCurrentState.EnterState();
    }

    public void Update()
    {
        if (npcCurrentState != null)
        {
            npcCurrentState.UpdateState();
        }
    }

    public bool IsPlayerInRange()
    {
        NPCController npc = GetComponent<NPCController>();
        return npc != null && npc.IsPlayerInRange();
    }   
}
